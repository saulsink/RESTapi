using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary;
using System.Net;

namespace EmployeesWebApplication.Controllers
{
    /// <summary>
    /// Employess controller.
    /// </summary>
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        /// <summary>
        /// Employees storage.
        /// </summary>        
        private readonly IEmployeeRepository _employeeRepository;

        /// <summary>
        /// Injecting dependencies into contructor.
        /// </summary>
        /// <param name="employeeRepository"></param>       
        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;            
        }

        /// <summary>
        /// GET api/employees
        /// List object records from memory.
        /// </summary>        
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Employee> employees = _employeeRepository.Get();  
            
            if (employees == null)
            {
                // The list is emppty.
                return NoContent();
            }
            else
            {
                // Return the list.
                return Ok(employees);
            }
        }

        /// <summary>
        /// GET api/employees/5
        /// Returns requested object.
        /// </summary>
        /// <param name="id"></param>        
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Employee employee = _employeeRepository.Get(id);
            
            if (employee == null)
            {
                // Entity not found by id.
                return NotFound(id);
            }
            else
            {
                // Return requested object.
                return Ok(employee);
            }
        }

        /// <summary>
        /// POST api/employees
        /// Stores arbitrary object into memory.
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public IActionResult Post([FromBody]Employee value)
        {
            Employee employee = _employeeRepository.Post(value);

            if (employee != null)
            {
                // Successfully created entity.                    
                return Created($"api/employees/{employee.Id}", value);
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Conflict;
                return BadRequest("Entity already exist in storage."); // TODO 409 Conflict response may be better
            }
        }

        /// <summary>
        /// DELETE api/employees/5
        /// Deletes selected object.
        /// </summary>
        /// <param name="id"></param>        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool deleted = _employeeRepository.Delete(id);

            if (deleted)
            {
                // Successfully deleted by id.
                return NoContent();
            }
            else
            {
                // Entity not found by id.
                return NotFound(id);
            }
        }
    }
}