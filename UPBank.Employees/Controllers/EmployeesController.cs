using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPBank.Addresses.Controllers;
using UPBank.Addresses.PostalServices;
using UPBank.Addresses.PostalServices.Abstract;
using UPBank.Employees.Data;
using UPBank.Employees.DTO;
using UPBank.Employees.Service;
using UPBank.Models;

namespace UPBank.Employees.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly UPBankEmployeesContext _context;
        private readonly EmployeeService _employeeService;

        public EmployeesController(UPBankEmployeesContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
        {
          if (_context.Employee == null)
          {
              return NotFound();
          }
            return await _context.Employee.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(string id)
        {
          if (_context.Employee == null)
          {
              return NotFound();
          }
            var employee = await _context.Employee.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(string id, Employee employee)
        {
            if (id != employee.Cpf)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees/Hire
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Hire")]
        public async Task<ActionResult<Employee>> HireEmployee(Employee employee)
        {
            if (_context.Employee == null)
            {
                return Problem("Entity set 'UPBankEmployeesContext.Employee' is null.");
            }
            if (ValidCPF(employee.Cpf) == false)
            {
                return BadRequest("Invalid CPF");
            }
            var newEmployee = new Employee
            {
                Cpf = new string(employee.Cpf.Where(char.IsDigit).ToArray()),
                Name = employee.Name,
                BirthDate = employee.BirthDate,
                Gender = employee.Gender,
                Salary = employee.Salary,
                Phone = employee.Phone,
                Email = employee.Email,
                Register = employee.Register,
                Manager = employee.Manager,
                // AgencyNumber = dto.AgencyNumber, // Devo manter?
                Agency = await _employeeService.GetAgencyAsync(employee.AgencyNumber)
            };

            if (!string.IsNullOrEmpty(employee.AddressZipcode))
            {

                string zipCode = new string(employee.AddressZipcode.Where(char.IsDigit).ToArray());

                var address = await new ViaCepService().Fetch(zipCode);

                if (address != null)
                {
                    newEmployee.Address = new Address
                    {
                        Zipcode = zipCode,
                        Street = address.Street,
                        City = address.City,
                        State = address.State,
                        Neighborhood = address.Neighborhood,
                        Number = employee.Address?.Number ?? 0,
                        Complement = employee.Address.Complement
                    };
                }
                else
                {
                    return BadRequest("Address not found for the provided ZIP code.");
                }
            }

            _context.Employee.Add(newEmployee);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EmployeeExists(newEmployee.Cpf))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEmployee", new { id = newEmployee.Cpf }, newEmployee);
        }


        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            if (_context.Employee == null)
            {
                return NotFound();
            }
            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(string id)
        {
            return (_context.Employee?.Any(e => e.Cpf == id)).GetValueOrDefault();
        }
        private bool ValidCPF(string cpf)
        {
            if (cpf.Length != 11)
                return false;

            foreach (char caracter in cpf)
                if (!char.IsDigit(caracter))
                    return false;

            int[] multiplicadoresPrimeiroDigito = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadoresSegundoDigito = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicadoresPrimeiroDigito[i];

            int resto = soma % 11;
            int primeiroDigitoVerificador = resto < 2 ? 0 : 11 - resto;

            tempCpf += primeiroDigitoVerificador;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicadoresSegundoDigito[i];

            resto = soma % 11;
            int segundoDigitoVerificador = resto < 2 ? 0 : 11 - resto;

            return cpf.EndsWith(primeiroDigitoVerificador.ToString() + segundoDigitoVerificador.ToString());
        }

    }
}
