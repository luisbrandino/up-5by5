using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPBank.Accounts.Api.Agency;
using UPBank.Accounts.Api.Customer;
using UPBank.Accounts.Controllers;
using UPBank.Accounts.Data;
using UPBank.Accounts.Requests;
using UPBank.Accounts.Services;
using UPBank.Addresses.Controllers;
using UPBank.Addresses.PostalServices;
using UPBank.Accounts.Specifications;
using UPBank.DTOs;
using UPBank.Models;

namespace UPBank.Tests.Accounts
{
    public class AccountControllerTest
    {
        private readonly DbContextOptions<UPBankAccountsContext> _options;

        public AccountControllerTest()
        {
            _options = new DbContextOptionsBuilder<UPBankAccountsContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new UPBankAccountsContext(_options);

            context.SaveChanges();
        }

        public AccountsController Make() => new AccountsController(
                new UPBankAccountsContext(_options),
                new AccountService(
                    new UPBankAccountsContext(_options),
                    new AccountCreationValidation(),
                    new MockCustomerApi(),
                    new MockAgencyApi()
                )
            );

        [Fact]
        public async Task Post_ValidAccountFields_ReturnsCreatedAccount()
        {
            var controller = Make();

            var accountCreationDTO = new AccountCreationDTO
            {
                Overdraft = 2000,
                Profile = Enums.EProfile.Normal,
                AgencyNumber = "10001",
                Customers = new List<string> { "000.000.000-01" }
            };

            var createdResult = await controller.Post(accountCreationDTO);

            var createdAccount = Assert.IsType<Account>(createdResult.Value);
            Assert.Equal(accountCreationDTO.Profile, createdAccount.Profile);
            Assert.Equal(accountCreationDTO.AgencyNumber, createdAccount.AgencyNumber);
            var context = new UPBankAccountsContext(_options);
            Assert.Equal(1, await context.Account.CountAsync());
            Assert.Equal(1, await context.AccountCustomer.CountAsync());
        }

        [Fact]
        public async Task Post_AgencyNotInDatabase_ReturnsBadRequest()
        {
            var controller = Make();

            var accountCreationDTO = new AccountCreationDTO
            {
                Overdraft = 2000,
                Profile = Enums.EProfile.Normal,
                AgencyNumber = "10008",
                Customers = new List<string> { "000.000.000-01" }
            };

            var createdResult = await controller.Post(accountCreationDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(createdResult.Result);
            var badRequestMessage = badRequestResult.Value;
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Agência não cadastrada", badRequestMessage);
        }

        [Fact]
        public async Task Post_ClientNotInDatabase_ReturnsBadRequest()
        {
            var controller = Make();

            var accountCreationDTO = new AccountCreationDTO
            {
                Overdraft = 2000,
                Profile = Enums.EProfile.Normal,
                AgencyNumber = "10001",
                Customers = new List<string> { "000.000.000-11" }
            };

            var createdResult = await controller.Post(accountCreationDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(createdResult.Result);
            var badRequestMessage = badRequestResult.Value;
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Cliente não cadastrado", badRequestMessage);
        }

        [Fact]
        public async Task Post_FirstClientIsMinor_ReturnsBadRequest()
        {
            var controller = Make();

            var accountCreationDTO = new AccountCreationDTO
            {
                Overdraft = 2000,
                Profile = Enums.EProfile.Normal,
                AgencyNumber = "10001",
                Customers = new List<string> { "000.000.000-02" }
            };

            var createdResult = await controller.Post(accountCreationDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(createdResult.Result);
            var badRequestMessage = badRequestResult.Value;
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("O primeiro cliente da conta deve ser maior de idade", badRequestMessage);
        }

        [Fact]
        public async Task Post_ValidAccountFieldsWithTwoClients_ReturnsCreatedAccount()
        {
            var controller = Make();

            var accountCreationDTO = new AccountCreationDTO
            {
                Overdraft = 2000,
                Profile = Enums.EProfile.Normal,
                AgencyNumber = "10001",
                Customers = new List<string> { "000.000.000-01", "000.000.000-03" }
            };

            var createdResult = await controller.Post(accountCreationDTO);

            var createdAccount = Assert.IsType<Account>(createdResult.Value);
            Assert.Equal(accountCreationDTO.Profile, createdAccount.Profile);
            Assert.Equal(accountCreationDTO.AgencyNumber, createdAccount.AgencyNumber);
            Assert.Equal(accountCreationDTO.Customers.Count, createdAccount.Customers.Count);
            var context = new UPBankAccountsContext(_options);
            Assert.Equal(1, await context.Account.CountAsync());
            Assert.Equal(2, await context.AccountCustomer.CountAsync());
        }

        [Fact]
        public async Task Post_ValidAccountFieldsWithMoreThanTwoClients_ReturnsBadRequest()
        {
            var controller = Make();

            var accountCreationDTO = new AccountCreationDTO
            {
                Overdraft = 2000,
                Profile = Enums.EProfile.Normal,
                AgencyNumber = "10001",
                Customers = new List<string> { "000.000.000-01", "000.000.000-02", "000.000.000-03" }
            };

            var createdResult = await controller.Post(accountCreationDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(createdResult.Result);
            var badRequestMessage = badRequestResult.Value;
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Uma conta pode ter no máximo dois clientes", badRequestMessage);
        }

        [Fact]
        public async Task Post_AccountWithDuplicateClients_ReturnsBadRequest()
        {
            var controller = Make();

            var accountCreationDTO = new AccountCreationDTO
            {
                Overdraft = 2000,
                Profile = Enums.EProfile.Normal,
                AgencyNumber = "10001",
                Customers = new List<string> { "000.000.000-01", "000.000.000-01" }
            };

            var createdResult = await controller.Post(accountCreationDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(createdResult.Result);
            var badRequestMessage = badRequestResult.Value;
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Não pode haver clientes repetidos", badRequestMessage);
        }
    }
}
