using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPBank.Addresses.Controllers;
using UPBank.Addresses.PostalServices;
using UPBank.Addresses.PostalServices.Abstract;
using UPBank.Employees.Data;
using UPBank.Employees.DTO;
using UPBank.Employees.Service;
using UPBank.Models.Utils;
using UPBank.Enums;
using UPBank.Models;
using UPBank.DTOs;
using System.Text;
using Newtonsoft.Json;

namespace UPBank.Employees.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly UPBankEmployeesContext _context;
        private readonly EmployeeService _employeeService;
        private readonly string _addressEndPoint = "https://localhost:7004";
        private readonly string _agencyEndPoint = "https://localhost:7059"; // Atualizar Endpoint
        private readonly string _accountEndPoint = "https://localhost:7193";

        public EmployeesController(UPBankEmployeesContext context)
        {
            _context = context;
        }

        // GET: https://localhost:7028/api/Employees/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
        {
            if (_context.Employee == null)
            {
                return Problem("Entity Set is null");
            }

            List<Employee> list = await _context.Employee.ToListAsync();

            foreach(Employee employee in list)
            {
                var address = await ApiConsume<Address>.Get(_addressEndPoint, $"api/addresses/zipcode/{employee.AddressZipcode}");
                employee.Address = address;
                var agency = await ApiConsume<Agency>.Get(_agencyEndPoint, $"api/agencies/agency/{employee.AgencyNumber}"); // Inserir a porta do endpoint de Agency
                employee.Agency = agency;
            }

            return list;
        }

        // GET: https://localhost:7028/api/Employees/cpf
        [HttpGet("{cpf}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(string cpf)
        {
            if (_context.Employee == null)
            {
                return Problem("Entity Set is null");
            }

            var employee = await _context.Employee.FindAsync(cpf);

            if (employee == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(employee.AddressZipcode))
            {
                var address = await ApiConsume<Address>.Get(_addressEndPoint, $"api/addresses/zipcode/{employee.AddressZipcode}");
                employee.Address = address;
            }

            if (!string.IsNullOrEmpty(employee.AgencyNumber))
            {
                var agency = await ApiConsume<Agency>.Get(_agencyEndPoint, $"api/agencies/agency/{employee.AgencyNumber}"); // Inserir a rota correta do endpoint de Agency
                employee.Agency = agency;
            }

            return Ok(employee);
        }

        [HttpGet("agency/{number}")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeesByAgency(string number)
        {
            if (_context.Employee == null)
            {
                return Problem("Entity Set is null");
            }

            var employees = await _context.Employee.Where(e => e.AgencyNumber == number).ToListAsync();

            if (employees == null)
            {
                return NotFound();
            }

            foreach (Employee employee in employees)
            {
                var address = await ApiConsume<Address>.Get(_addressEndPoint, $"api/addresses/zipcode/{employee.AddressZipcode}");
                employee.Address = address;
            }

            return Ok(employees);
        }

        // PUT: api/Employees/5
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

        // POST https://localhost:7028/api/Employees/Hire
        [HttpPost("Hire")]
        #region Post Example for the https://localhost:7028/api/Employees/Hire URL
        /*
         {
          "Cpf": "61235000079",
          "Name": "João Silva",
          "Gender": "M",
          "Salary": 5000.00,
          "Phone": "11987654321",
          "Email": "joao.silva@example.com",
          "BirthDate": "2000-01-01",
          "AddressZipcode": "14805-295",
          "Manager": true,
          "AgencyNumber": "001"
         }
         */
        #endregion
        public async Task<ActionResult<Employee>> HireEmployee(EmployeeDTO dto)
        {
            if (_context.Employee == null)
            {
                return Problem("Entity set 'FlightsApiContext.Flights' is null.");
            }

            var address = await ApiConsume<Address>.Get(_addressEndPoint, $"api/addresses/zipcode/{dto.AddressZipcode}");

            if (address == null)
                address = await ApiConsume<Address>.Post(_addressEndPoint, $"/api/addresses", new AddressDTO { Zipcode = dto.AddressZipcode, Number = 1, Complement = ""});

            if (address == null)
                return BadRequest($"Invalid zipcode: {dto.AddressZipcode}");

            var agency = await ApiConsume<Agency>.Get(_agencyEndPoint, $"api/agencies/agency/{dto.AgencyNumber}"); // Inserir a porta do endpoint de Agency
            
            dto.Cpf = new string(dto.Cpf.Where(char.IsDigit).ToArray());
            if (!ValidCPF(dto.Cpf))
            {
                return BadRequest($"Invalid cpf: {dto.Cpf}");
            }
            Employee employee = new Employee();
            #region Saving Variables
            employee.Address = address;
            employee.Agency = agency;
            employee.Cpf = dto.Cpf;
            employee.Name = dto.Name;
            employee.Gender = dto.Gender;
            employee.Salary = dto.Salary;
            employee.Phone = dto.Phone;
            employee.Email = dto.Email;
            employee.BirthDate = dto.BirthDate;
            employee.AddressZipcode = dto.AddressZipcode;
            employee.Manager = dto.Manager;
            employee.AgencyNumber = dto.AgencyNumber;
            employee.Register = 1;
            #endregion
            _context.Employee.Add(employee);
            await _context.SaveChangesAsync();
            return Ok(employee);
        }


        // PATCH https://localhost:7028/api/Employees/{OptionType}/EmployeeCPF    // SetProfile or ApproveAccount
        [HttpPatch("{op}")]
        #region Body Example for implementation
        /*
         {
            "Restriction": false,
            "AgencyNumber": "001",
            "CustomerCPF": "65076248024",
            "EmployeeCPF": "84113873054",
            "AccountNumber":"1234"
         }
         */
        #endregion
        public async Task<Account> AlterAccount(AccountDTO bodyAccount,string op)
        {
            // Utilização de Get by Id para operações
            var employee = await ApiConsume<Employee>.Get("https://localhost:7028/api/Employees/", bodyAccount.EmployeeCPF);
            var account = await ApiConsume<Account>.Get(_accountEndPoint, $"api/accounts/{bodyAccount.AccountNumber}");

            var customer = account.Customers.First();
            if (op == "SetProfile")
            {
                if (customer.Salary <= 1500)
                {
                    account.Profile = EProfile.University;
                }
                else if (customer.Salary > 1500 && customer.Salary <= 10000)
                {
                    account.Profile = EProfile.Normal;
                }
                else if (customer.Salary > 10000)
                {
                    account.Profile = EProfile.Vip;
                }

                HttpClient client = new HttpClient();

                string jsonContent = JsonConvert.SerializeObject(account);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync($"{_accountEndPoint}api/accounts/{bodyAccount.AccountNumber}/profile", content);
                response.EnsureSuccessStatusCode();
            }
            if (employee.Manager == true && op == "ApproveAccount")
            {
                account.Restriction = !account.Restriction;

                var newAccount = await ApiConsume<Account>.Post(_accountEndPoint, $"api/accounts/{bodyAccount.AccountNumber}/activate", null);

            }

            return account;
        }

        // DELETE: https://localhost:7028/api/Employees/{cpf}
        [HttpDelete("{cpf}")]
        public async Task<IActionResult> DeleteEmployee(string cpf)
        {
            if (_context.Employee == null)
            {
                return NotFound("Not found a cpf");
            }
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var employee = await _context.Employee.FindAsync(cpf);
                if (employee == null)
                {
                    return NotFound("");
                }
                var deletedEmployee = CreateDeletedEmployeeFromEmployee(employee);
                _context.DeletedEmployee.Add(deletedEmployee);
                _context.Employee.Remove(employee);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return NoContent();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Internal server error");
            }
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
        public static DeletedEmployee CreateDeletedEmployeeFromEmployee(Employee employee)
        {
            return new DeletedEmployee
            {
                Cpf = employee.Cpf,
                Manager = employee.Manager,
                AgencyNumber = employee.AgencyNumber,
                Name = employee.Name,
                BirthDate = employee.BirthDate,
                Gender = employee.Gender,
                Salary = employee.Salary,
                Phone = employee.Phone,
                Email = employee.Email,
                AddressZipcode = employee.AddressZipcode
            };
        }
    }
}
