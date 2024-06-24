using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using UPBank.Customers.Controllers;
using UPBank.Customers.Services;
using UPBank.DTOs;
using UPBank.Models;
using Xunit;

namespace UPBank.Tests.Customers
{
    public class CustomerControllerTest
    {
        private readonly Mock<CustomerService> _mockCustomerService;
        private readonly CustomersController _controller;

        public CustomerControllerTest()
        {
            _mockCustomerService = new Mock<CustomerService>();
            _controller = new CustomersController(_mockCustomerService.Object);
        }

        [Fact]
        public async Task PostCustomer_CPFInvalido_RetornaBadRequest()
        {
            var dto = new CustomersDTO
            {
                Cpf = "1234567890", // CPF inválido
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
            var existingCustomer = new Customer { Cpf = "46067325802", Name = "Test Customer" };
            _mockCustomerService.Setup(service => service.GetByCpf("46067325802")).ReturnsAsync(existingCustomer);

            var result = await _controller.GetCustomerByCpf("46067325802");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var customer = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal(existingCustomer.Cpf, customer.Cpf);
            Assert.Equal(existingCustomer.Name, customer.Name);
        }

        [Fact]
        public async Task EditCustomer_ClienteExistente_RetornaNoContent()
        {
            var dto = new CustomersDTO { Cpf = "46067325802", Name = "Updated Customer" };

            _mockCustomerService.Setup(service => service.EditCustomer(It.IsAny<CustomersDTO>())).ReturnsAsync(true);

            var result = await _controller.EditCustomer("46067325802", dto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task ChangeRestriction_ClienteExistente_RetornaNoContent()
        {
            _mockCustomerService.Setup(service => service.ChangeRestriction("46067325802")).ReturnsAsync(true);

            var result = await _controller.ChangeRestriction("46067325802");

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCustomer_ClienteExistente_RetornaOk()
        {
            var existingCustomer = new Customer { Cpf = "46067325802", Name = "Test Customer" };
            _mockCustomerService.Setup(service => service.DeleteCustomer("46067325802")).ReturnsAsync(existingCustomer);

            var result = await _controller.DeleteCustomer("46067325802");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var customer = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal(existingCustomer.Cpf, customer.Cpf);
        }
    }
}