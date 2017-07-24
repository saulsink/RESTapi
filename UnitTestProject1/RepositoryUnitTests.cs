using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class EmployeesRepositoryUnitTests
    {
        private readonly IEmployeeRepository _employeeRepository;

        /// <summary>
        /// Used for seeding data storage.
        /// </summary>
        Dictionary<int, Employee> _employeesForPost;

        public EmployeesRepositoryUnitTests()
        {
            // Choose repository by uncommenting: EmployeeRedisRepository or EmployeeTestRepository            
            //_employeeRepository = new EmployeeRedisRepository();
            _employeeRepository = new EmployeeTestRepository();

            // Add some employees to storage for testing cases. 
            // Using MaxValue in case the tests will be run on production data.
            _employeesForPost = new Dictionary<int, Employee>();            
            _employeesForPost.Add(System.Int32.MaxValue, new Employee(System.Int32.MaxValue, "Joe"));
            _employeesForPost.Add(System.Int32.MaxValue - 1, new Employee(System.Int32.MaxValue - 1, "Andrew"));
            _employeesForPost.Add(System.Int32.MaxValue - 2, new Employee(System.Int32.MaxValue - 2, "Petr"));
        }

        /// <summary>
        /// Clean up the storage after tests are done.
        /// </summary>
        ~EmployeesRepositoryUnitTests()
        {
            _employeeRepository.Delete(System.Int32.MaxValue);
            _employeeRepository.Delete(System.Int32.MaxValue - 1);
            _employeeRepository.Delete(System.Int32.MaxValue - 2);
            _employeeRepository.Delete(System.Int32.MaxValue - 3);           
        }

        /// <summary>
        /// Check if Post(employeeSerialized) stores new entity and Get(id) retrieves it by id.
        /// </summary>
        [TestMethod]
        public void AddNewEmployee()
        {
            int id = System.Int32.MaxValue - 3;

            // Add new Employee.
            Employee employeeForPost = new Employee(id, "Saulius Sinkevièius");
            _employeeRepository.Post(employeeForPost);            

            // Retrieve new Employee by its id.
            Employee newEmployee = _employeeRepository.Get(id);            

            // Should retrieve the same.
            Assert.AreEqual(employeeForPost.ToString(), newEmployee.ToString());
        }

        /// <summary>
        /// Check Get(id) if entity if such id does not exist.
        /// </summary>
        [TestMethod]
        public void NotFoundInRepository()
        {
            Employee employee = _employeeRepository.Get(System.Int32.MaxValue - 4);

            Assert.IsNull(employee);
        }

        /// <summary>
        /// Check if Get() retrieves.
        /// </summary>
        [TestMethod]
        public void AddAFewEmployees()
        {
            // Post all testing entities.
            foreach (var employeeForPost in _employeesForPost)
            {
                _employeeRepository.Post(employeeForPost.Value);
            }

            // Get all employees from storage.
            IEnumerable<Employee> employees = _employeeRepository.Get();

            // Count if all were retrieved.
            int result = 0;
            using (IEnumerator<Employee> enumerator = employees.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (_employeesForPost.ContainsKey(enumerator.Current.Id))
                        result++;
                }
            }

            // All should be retrieved.
            Assert.AreEqual(_employeesForPost.Count, result);
        }

        /// <summary>
        /// Check if Delete(id) removes entities.
        /// </summary>
        [TestMethod]
        public void DeleteEmployeeById()
        {
            // Post all testing entities
            List<int> ids = new List<int>();
            foreach (var employeeForPost in _employeesForPost)
            {
                _employeeRepository.Post(employeeForPost.Value);
                ids.Add(employeeForPost.Key);
            }

            // Delete the second.
            int idx = 1;
            _employeeRepository.Delete(ids[idx]);           

            // Get all employees from storage.
            IEnumerable<Employee> employees = _employeeRepository.Get();

            // Check if deletion was correct.
            int notDeletedCount = 0;
            bool correctDeletion = true;
            using (IEnumerator<Employee> enumerator = employees.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    // Count if exist in both.
                    if (_employeesForPost.ContainsKey(enumerator.Current.Id))
                        notDeletedCount++;

                    // If still exist in storage then it is not deleted.
                    if (enumerator.Current.Id == ids[idx])
                        correctDeletion = false;
                }
            }

            // Not deleted count should be less by one and correctDeletion should be true.
            Assert.IsTrue(notDeletedCount == _employeesForPost.Count - 1 && correctDeletion);
        }
    }
}
