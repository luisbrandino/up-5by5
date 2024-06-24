using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UPBank.Customers.Controllers;
using UPBank.Customers.Data;
using UPBank.Customers.Services;
using UPBank.DTOs;
using UPBank.Models;
using Xunit;

namespace UPBank.Tests.Customers
{
    public class CustomerControllerTest
    {
        private readonly UPBankCustomersContext _context;
        private readonly CustomersController _controller;

        public CustomerControllerTest()
        {

            var options = new DbContextOptionsBuilder<UPBankCustomersContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new UPBankCustomersContext(options); 

            var address1 = new Address
            {
                Zipcode = "12345678",
                Number = 123,
                Complement = "Apto 101",
                Street = "Rua Principal",
                City = "São Paulo",
                State = "SP",
                Neighborhood = "Centro"
            };

            var address2 = new Address
            {
                Zipcode = "87654321",
                Number = 456,
                Complement = "Casa 102",
                Street = "Rua Secundária",
                City = "Rio de Janeiro",
                State = "RJ",
                Neighborhood = "Bairro Secundário"
            };

            _context.Addresses.AddRange(address1, address2);

            _context = new UPBankCustomersContext(options);

            var customer1 = new Customer
            {
                Cpf = "46067325802",
                Name = "Luan",
                Email = "teste@exemplo.com",
                Phone = "12345678901",
                Address = address1, // Associe o endereço 1 ao cliente 1
                Salary = 1000,
                BirthDate = DateTime.Now.AddYears(-25)
            };

            var customer2 = new Customer
            {
                Cpf = "98911729027",
                Name = "Luan",
                Email = "test2@exemplo.com",
                Phone = "09876543210",
                Address = address2, // Associe o endereço 2 ao cliente 2
                Salary = 2000,
                BirthDate = DateTime.Now.AddYears(-30)
            };

            _context.Customer.AddRange(customer1, customer2);
            _context.SaveChanges();

            var customerService = new CustomerService();
            _controller = new CustomersController(customerService);
        }

        [Fact]
        public async Task PostCustomer_CPFInvalido_RetornaBadRequest()
        {
            var dto = new CustomersDTO
            {
                Cpf = "1234567890", // CPF inválido
                Name = "Luan",
                Email = "teste@exemplo.com",
                Phone = "12345678901",
                Address = "12345",
                Salary = 1000,
                BirthDate = DateTime.Now.AddYears(-25)
            };

            var resultado = await _controller.PostCustomer(dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.Equal("Dados Invalidos", badRequestResult.Value);
        }

        [Fact]
        public async Task PostCustomer_EmailInvalido_RetornaBadRequest()
        {
            var dto = new CustomersDTO
            {
                Cpf = "46067325802",
                Name = "Luan",
                Email = "email-invalido", //email inválido
                Phone = "12345678901",
                Address = "12345",
                Salary = 1000,
                BirthDate = DateTime.Now.AddYears(-25)
            };

            var resultado = await _controller.PostCustomer(dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.Equal("Dados Invalidos", badRequestResult.Value);
        }

        [Fact]
        public async Task PostCustomer_TelefoneInvalido_RetornaBadRequest()
        {
            var dto = new CustomersDTO
            {
                Cpf = "46067325802",
                Name = "Luan",
                Email = "teste@exemplo.com",
                Phone = "123456789", // Telefone inválido
                Address = "12345",
                Salary = 1000,
                BirthDate = DateTime.Now.AddYears(-25)
            };

            var resultado = await _controller.PostCustomer(dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.Equal("Dados Invalidos", badRequestResult.Value);
        }

        [Fact]
        public async Task PostCustomer_SalarioInvalido_RetornaBadRequest()
        {
            var dto = new CustomersDTO
            {
                Cpf = "46067325802",
                Name = "Luan",
                Email = "teste@exemplo.com",
                Phone = "12345678901",
                Address = "12345",
                Salary = -1000, // Salário inválido
                BirthDate = DateTime.Now.AddYears(-25)
            };

            var resultado = await _controller.PostCustomer(dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.Equal("Dados Invalidos", badRequestResult.Value);
        }

        [Fact]
        public async Task PostCustomer_DataNascimentoInvalida_RetornaBadRequest()
        {
            var dto = new CustomersDTO
            {
                Cpf = "46067325802",
                Name = "Luan",
                Email = "teste@exemplo.com",
                Phone = "12345678901",
                Address = "12345",
                Salary = 1000,
                BirthDate = DateTime.Now.AddYears(1) // Data de nascimento inválida
            };

            var resultado = await _controller.PostCustomer(dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.Equal("Dados Invalidos", badRequestResult.Value);
        }

        [Fact]
        public async Task GetCustomerByCpf_ClienteExistente_RetornaOk()
        {
            var result = await _controller.GetCustomerByCpf("46067325802");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var customer = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal("46067325802", customer.Cpf);
        }

        [Fact]
        public async Task EditCustomer_ClienteExistente_RetornaNoContent()
        {
            var dto = new CustomersDTO
            {
                Cpf = "46067325802",
                Name = "Updated Customer",
                Email = "updated@exemplo.com",
                Phone = "12345678901",
                Address = "Updated Address",
                Salary = 2000,
                BirthDate = DateTime.Now.AddYears(-26)
            };

            var result = await _controller.EditCustomer("46067325802", dto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCustomer_ClienteExistente_RetornaOk()
        {
            var result = await _controller.DeleteCustomer("98911729027");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var customer = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal("98911729027", customer.Cpf);
        }
    }
}