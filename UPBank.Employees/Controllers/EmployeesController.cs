using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPBank.Addresses.Controllers;
using UPBank.Addresses.PostalServices;
using UPBank.Addresses.PostalServices.Abstract;
using UPBank.Employees.Data;
using UPBank.Employees.Service;
using UPBank.Employees.Utils;
using UPBank.Models;

namespace UPBank.Employees.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly UPBankEmployeesContext _context;
        private readonly EmployeeService _employeeService;
        private readonly string _addressUri = "https://localhost:7004/";

        public EmployeesController(UPBankEmployeesContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        #region Teste Atual
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
        {
            if (_context.Employee == null)
            {
                return Problem("Entity Set is null");
            }

            List<Employee> list = await _context.Employee.Include(a => a.Address).Include(a => a.Agency).ToListAsync();

            return list;
        }
        #endregion
        #region Meu código
        //public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
        //{
        //  if (_context.Employee == null)
        //  {
        //      return Problem("Entity Set is null");
        //  }

        //    List<Employee> list = await _context.Employee.Include(a => a.Address).Include(a => a.Agency).ToListAsync();

        //    return list;
        //}
        #endregion
        #region Código Luan
        /*
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
        {
            if (_context.Employee == null)
            {
                return Problem("Entity Set is null");
            }
            // Recupera a lista de funcionários do banco de dados.
            var employees = await _context.Employee.ToListAsync();
            // Lista para armazenar os objetos Employee mapeados.
            var mappedEmployees = new List<Employee>();
            // Para cada funcionário, mapeia-o para um objeto Employee.
            foreach (var employee in employees)
            {
                // Cria um novo objeto Employee.
                var mappedEmployee = new Employee
                {
                    Cpf = employee.Cpf,
                    Name = employee.Name,
                };
                // Se o funcionário tiver um endereço associado, tenta obter o endereço da API.
                if (!string.IsNullOrEmpty(employee.AddressZipcode))
                {
                    // Faz a chamada à API para obter o endereço com base no código postal do funcionário.
                    var addressResult = await ApiConsume<List<Address>>.Get(_addressUri, $"/api/addresses/{employee.AddressZipcode}");
                    // Se o endereço for encontrado, mapeia-o para o objeto Address e atribui ao funcionário.
                    if (addressResult != null && addressResult.Any())
                    {
                        var address = addressResult.First(); // Supondo que a API retorna apenas um endereço.
                        mappedEmployee.Address = new Address
                        {
                            Zipcode = address.Zipcode,
                            Street = address.Street,
                            City = address.City,
                            State = address.State,
                            Neighborhood = address.Neighborhood,
                            Number = employee.Address.Number,
                            Complement = employee.Address.Complement
                        };
                    }
                }
                // Adiciona o objeto Employee mapeado à lista de funcionários mapeados.
                mappedEmployees.Add(mappedEmployee);
            }
            return mappedEmployees;
        }
        */
        #endregion

        // GET: api/Employees/5
        [HttpGet("{cpf}")]
        public async Task<ActionResult<Employee>> GetEmployee(string cpf)
        {
          if (_context.Employee == null)
          {
              return NotFound();
          }
            var employee = await _context.Employee.FindAsync(cpf);

            if (cpf == null)
            {
                return NotFound();
            }

            Employee? hiredEmployee = await _context.Employee.Include(a => a.Agency).FirstOrDefaultAsync(p => p.Cpf == cpf);

            if(hiredEmployee == null)
            {
                return NotFound("There isn't an active hired worker with this identification");
            }

            return hiredEmployee;
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
                AgencyNumber = employee.AgencyNumber, // Devo manter?
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
