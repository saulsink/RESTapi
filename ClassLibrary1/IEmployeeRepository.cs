using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    // Employee repository interface.
    public interface IEmployeeRepository
    {        
        /// <summary>
        /// Get all Employees from storage.
        /// </summary>
        /// <returns>List of Employees.</returns>
        IEnumerable<Employee> Get();

        /// <summary>
        /// Get one entity by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Employee.</returns>
        Employee Get(int id);

        /// <summary>
        /// Store one entity.
        /// </summary>
        /// <param name="Employee"></param>
        /// <returns>Returns new employee or null if it already exists.</returns>
        Employee Post(Employee employee);

        /// <summary>
        /// Delete one entity by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>If deleted then returns true, if not found then returns false.</returns>
        bool Delete(int id);
    }
}