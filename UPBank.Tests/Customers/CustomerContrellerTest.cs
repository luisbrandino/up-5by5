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
        public async Task PostCustomer_InvalidCPF_BadRequest()
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

            var result = await _controller.PostCustomer(dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Dados Invalidos", badRequestResult.Value);
        }

        [Fact]
        public async Task PostCustomer_InvalidEmail_BadRequest()
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

            var result = await _controller.PostCustomer(dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Dados Invalidos", badRequestResult.Value);
        }

        [Fact]
        public async Task PostCustomer_InvalidPhone_BadRequest()
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

            var result = await _controller.PostCustomer(dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Dados Invalidos", badRequestResult.Value);
        }

        [Fact]
        public async Task PostCustomer_InvalidSalary_BadRequest()
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

            var result = await _controller.PostCustomer(dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Dados Invalidos", badRequestResult.Value);
        }

        [Fact]
        public async Task PostCustomer_InvalidBirthDay_RetornaBadRequest()
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

            var result = await _controller.PostCustomer(dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Dados Invalidos", badRequestResult.Value);
        }

    }
}