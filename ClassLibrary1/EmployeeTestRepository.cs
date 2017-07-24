using System;
using System.Collections.Generic;
using System.Text;
using ClassLibrary;

namespace UnitTests
{
    public class EmployeeTestRepository : IEmployeeRepository
    {
        // Temporal storage.
        private Dictionary<int, Employee> _employees = new Dictionary<int, Employee>();
        
        public bool Delete(int id)
        {
            if (_employees.ContainsKey(id))
            {
                _employees.Remove(id);
                return true;
            }

            // Not found.
            return false;
        }

        public IEnumerable<Employee> Get()
        {
            List<Employee> employees = new List<Employee>();

            // Convert from Dictionary to List.
            foreach (var employee in _employees)
            {
                employees.Add(employee.Value);
            }

            if (employees.Count > 0)
                return employees;

            // Empty.
            return null;
        }

        public Employee Get(int id)
        {
            Employee employee = null;

            _employees.TryGetValue(id, out employee);

            return employee;
        }

        public Employee Post(Employee employee)
        {            
            _employees.Add(employee.Id, employee);

            return employee;
        }
    }
}
