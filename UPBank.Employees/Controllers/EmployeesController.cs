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
        private readonly string _addressEndPoint = "https://localhost:7004";
        private readonly string _agencyEndPoint = "https://localhost:7004"; // Atualizar Endpoint

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
                var agency = await ApiConsume<Agency>.Get(_agencyEndPoint, $"api/.../{employee.AgencyNumber}"); // Inserir a porta do endpoint de Agency
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
                var agency = await ApiConsume<Agency>.Get(_agencyEndPoint, $"api/agency/{employee.AgencyNumber}"); // Inserir a rota correta do endpoint de Agency
                employee.Agency = agency;
            }

            return Ok(employee);
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
            var agency = await ApiConsume<Agency>.Get(_agencyEndPoint, $"api/.../{dto.AgencyNumber}"); // Inserir a porta do endpoint de Agency
            
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
            return Ok($"Funcionário: {dto.Name} incluído no sistema");
        }


        // PATCH https://localhost:7028/api/Employees/Hire
        [HttpPatch("SetProfile/{EmployeeCPF}/{AccountNumber}/{ClientCPF}")]
        
        public async Task<Account> ApproveAccount(AccountDTO bodyAccount,string EmployeeCPF, string AccountNumber, string ClientCPF)
        {
            var agency = await ApiConsume<Agency>.Get(_agencyEndPoint, $"api/.../{bodyAccount.AgencyNumber}"); // Inserir a porta do endpoint de Agency


            Account account = new Account();
            return account;
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
