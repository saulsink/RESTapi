using System;
using System.Collections.Generic;
using System.Text;
using ServiceStack.Redis;
using Newtonsoft.Json;

namespace ClassLibrary
{
    public class EmployeeRedisRepository : IEmployeeRepository
    {
        private readonly string _entityName = typeof(Employee).Name;
        private readonly IRedisClient _redis;

        public EmployeeRedisRepository()
        {
            var manager = new RedisManagerPool("localhost:6379"); // TODO load server address from config
            _redis = manager.GetClient();
        }
        
        public IEnumerable<Employee> Get()
        {
            List <string> employeesSerialized = _redis.GetHashValues(_entityName);
            List<Employee> employees = new List<Employee>();

            // Deserialize all.
            foreach (var employeeSerialized in employeesSerialized)
            {
                employees.Add(JsonConvert.DeserializeObject<Employee>(employeeSerialized));
            }            

            return employees;
        }

        public Employee Get(int id)
        {
            string employee;
            
            // If exist then try to get and deserialize.
            if (_redis.Hashes[_entityName].ContainsKey(id.ToString()))
            {
                _redis.Hashes[_entityName].TryGetValue(id.ToString(), out employee);

                return JsonConvert.DeserializeObject<Employee>(employee);                
            }

            return null;
        }

        public Employee Post(Employee employee)
        { 
            // Post only if does not exist already.
            if (!_redis.Hashes[_entityName].ContainsKey(employee.Id.ToString()))
            {
                _redis.Hashes[_entityName].Add(employee.Id.ToString(), JsonConvert.SerializeObject(employee));
                return employee;
            }

            return null;
        }

        public bool Delete(int id)
        {
            // If exist then delete.
            if (_redis.Hashes[_entityName].ContainsKey(id.ToString()))
            {
                _redis.Hashes[_entityName].Remove(id.ToString());
                return true;
            }

            return false;
        }
    }
}