using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using UPBank.Customers.Services;
using UPBank.DTOs;
using UPBank.Models;

namespace UPBank.Customers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public CustomersController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet] // GET: /api/customers
        public async Task<ActionResult<List<Customer>>> GetAllCustomers()
        {
            try
            {
                var customers = await _customerService.GetAll();
                if (customers == null)
                {
                    return NotFound("Clientes não encontrados");
                }
                return customers;

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{cpf}")] // GET: /api/customers/140.846.310-51
        public async Task<ActionResult<Customer>> GetCustomerByCpf(string cpf)
        {
            var trateCPF = new string(cpf.Where(char.IsDigit).ToArray());
            try
            {
                var customer = await _customerService.GetByCpf(trateCPF);
                if (customer == null)
                {
                    return NotFound("Cliente não encontrado");
                }
                return customer;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost] // POST: /api/customers
        public async Task<ActionResult<Customer>> PostCustomer(CustomersDTO dto)
        {
            if (dto == null)
                return BadRequest("Nulo");

            dto.Cpf = new string(dto.Cpf.Where(char.IsDigit).ToArray());
            dto.Phone = new string(dto.Phone.Where(char.IsDigit).ToArray());

            if (dto.Address.All(char.IsDigit))
                dto.Address = dto.Address.Insert(dto.Address.Length - 3, "-");

            if (!ValidCPF(dto.Cpf) || !ValidEmail(dto.Email) || !ValidPhone(dto.Phone) || !validSalary(dto.Salary) || !validBirthDay(dto.BirthDate))
                return BadRequest("Dados Invalidos");

            try
            {
               
                var customer = await _customerService.PostCustomer(dto);

                if (customer == null)
                {
                    return BadRequest("Nenhum Cliente Enviado");
                }
                return CreatedAtAction(nameof(GetCustomerByCpf), new { cpf = customer.Cpf }, customer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPut("{cpf}")] // PUT: /api/customers/140.846.310-51
        public async Task<IActionResult> EditCustomer(string cpf, CustomersDTO dto)
        {

            var trateCPF = new string(cpf.Where(char.IsDigit).ToArray());
            dto.Cpf = new string(dto.Cpf.Where(char.IsDigit).ToArray());

            if (trateCPF != dto.Cpf)
            {
                return BadRequest("Cpfs Diferentes");
            }
            try
            {
                var result = await _customerService.EditCustomer(dto);

                if (!result)
                {
                    return NotFound("Não encontrado");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{cpf}")] // DELETE: /api/customers/140.846.310-51
        public async Task<ActionResult<Customer>> DeleteCustomer(string cpf)
        {
            var trateCPF = new string(cpf.Where(char.IsDigit).ToArray());

            try
            {
                var customer = await _customerService.DeleteCustomer(trateCPF);
                if (customer == null)
                {
                    return NotFound("Cliente não encontrado");
                }
                return customer;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch("{cpf}/changerestriction")] // PATCH: /api/customers/140.846.310-51 
        public async Task<IActionResult> ChangeRestriction(string cpf)
        {
            var trateCPF = new string(cpf.Where(char.IsDigit).ToArray());

            try
            {
                var result = await _customerService.ChangeRestriction(trateCPF);
                if (!result)
                {
                    return NotFound("Cliente não encontrado");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

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
        private bool validBirthDay(DateTime birthDate)
        {
            DateTime currentDate = DateTime.Now;
            if (birthDate > currentDate)
                return false;

            return true;

        }

        private bool validSalary(double salary)
        {
            if (salary > 0)
                return true;

            return false;
        }

        private bool ValidPhone(string phone)
        {
            if (phone.Length != 11)
                return false;

            return true;
        }

        private bool ValidEmail(string email)
        {
            string ValidFormat = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (Regex.IsMatch(email, ValidFormat))
                return true;

            return false;

        }

    }
}
