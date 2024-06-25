using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPBank.Accounts.Api.Agency;
using UPBank.Accounts.Api.Customer;
using UPBank.Accounts.Controllers;
using UPBank.Accounts.Data;
using UPBank.Accounts.Services;
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

            context.Account.Add(new Account
            {
                Overdraft = 2000,
                Profile = Enums.EProfile.Normal,
                AgencyNumber = "10301",
                Customers = new List<Customer>(),
                Number = "123456789",
                CreditCardNumber = 2020,
                Balance = 2000,
                Restriction = true
            });

            context.Account.Add(new Account
            {
                Overdraft = 1000,
                Profile = Enums.EProfile.Normal,
                AgencyNumber = "10801",
                Customers = new List<Customer>(),
                Number = "505020",
                CreditCardNumber = 1111,
                Balance = 1111,
                Restriction = true
            });

            context.SaveChanges();
        }

        public AccountsController Make()
        {
            var context = new UPBankAccountsContext(_options);
            var accountService = new AccountService(
                    context,
                    new CreditCardService(context),
                    new MockCustomerApi(),
                    new MockAgencyApi()
                );

            return new AccountsController(
                context,
                accountService,
                new TransactionService(context, accountService)
            );

        }

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
            Assert.Equal(3, await context.Account.CountAsync());
            Assert.Equal(1, await context.AccountCustomer.CountAsync());
            Assert.Equal(1, await context.CreditCard.CountAsync());
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
            Assert.Equal(3, await context.Account.CountAsync());
            Assert.Equal(2, await context.AccountCustomer.CountAsync());
            Assert.Equal(1, await context.CreditCard.CountAsync());
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

        [Fact]
        public async Task Post_TransactionWithNegativeValue_ReturnsBadRequest()
        {
            var controller = Make();

            var transactionCreationDto = new TransactionCreationDTO
            {
                Type = Enums.EType.Loan,
                OriginNumber = "123456789",
                Value = -10
            };

            var createdResult = await controller.MakeTransaction(transactionCreationDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(createdResult.Result);
            var badRequestMessage = badRequestResult.Value;
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Valor da transação tem que ser positivo", badRequestMessage);
        }

        [Fact]
        public async Task Post_TransactionOriginAccountNotInDatabase_ReturnsBadRequest()
        {
            var controller = Make();

            var transactionCreationDto = new TransactionCreationDTO
            {
                Type = Enums.EType.Loan,
                OriginNumber = "1231312313",
                Value = 1000
            };

            var createdResult = await controller.MakeTransaction(transactionCreationDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(createdResult.Result);
            var badRequestMessage = badRequestResult.Value;
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Conta de origem não cadastrada", badRequestMessage);
        }

        [Fact]
        public async Task Post_ValidTransaction_ReturnsCreatedTransaction()
        {
            var controller = Make();

            var transactionCreationDto = new TransactionCreationDTO
            {
                Type = Enums.EType.Loan,
                OriginNumber = "123456789",
                Value = 1000
            };

            var createdResult = await controller.MakeTransaction(transactionCreationDto);

            var createdTransaction = Assert.IsType<Transaction>(createdResult.Value);
            Assert.Equal(transactionCreationDto.Type, createdTransaction.Type);
            Assert.Equal(transactionCreationDto.Value, createdTransaction.Value);
            Assert.Equal(transactionCreationDto.OriginNumber, createdTransaction.OriginNumber);
            var context = new UPBankAccountsContext(_options);
            Assert.Equal(1, await context.Transaction.CountAsync());
        }

        [Fact]
        public async Task Post_PaymentTransactionWithNoDestinyAccount_ReturnsBadRequest()
        {
            var controller = Make();

            var transactionCreationDto = new TransactionCreationDTO
            {
                Type = Enums.EType.Payment,
                OriginNumber = "123456789",
                Value = 1000
            };

            var createdResult = await controller.MakeTransaction(transactionCreationDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(createdResult.Result);
            var badRequestMessage = badRequestResult.Value;
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Conta de destino não cadastrada", badRequestMessage);
        }

        [Fact]
        public async Task Post_PaymentTransactionWithInvalidDestinyAccount_ReturnsBadRequest()
        {
            var controller = Make();

            var transactionCreationDto = new TransactionCreationDTO
            {
                Type = Enums.EType.Payment,
                OriginNumber = "123456789",
                DestinyNumber = "23948209348",
                Value = 1000
            };

            var createdResult = await controller.MakeTransaction(transactionCreationDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(createdResult.Result);
            var badRequestMessage = badRequestResult.Value;
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Conta de destino não cadastrada", badRequestMessage);
        }

        [Fact]
        public async Task Post_PaymentTransactionWithDuplicateOriginAndDestinyAccount_ReturnsBadRequest()
        {
            var controller = Make();

            var transactionCreationDto = new TransactionCreationDTO
            {
                Type = Enums.EType.Payment,
                OriginNumber = "123456789",
                DestinyNumber = "123456789",
                Value = 1000
            };

            var createdResult = await controller.MakeTransaction(transactionCreationDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(createdResult.Result);
            var badRequestMessage = badRequestResult.Value;
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Conta de origem não pode ser a mesma que conta de destino", badRequestMessage);
        }

        [Fact]
        public async Task Post_ValidPaymentTransaction_ReturnsCreatedTransaction()
        {
            var controller = Make();

            var transactionCreationDto = new TransactionCreationDTO
            {
                Type = Enums.EType.Payment,
                OriginNumber = "123456789",
                DestinyNumber = "505020",
                Value = 1000
            };

            var createdResult = await controller.MakeTransaction(transactionCreationDto);

            var createdTransaction = Assert.IsType<Transaction>(createdResult.Value);
            Assert.Equal(transactionCreationDto.Type, createdTransaction.Type);
            Assert.Equal(transactionCreationDto.Value, createdTransaction.Value);
            Assert.Equal(transactionCreationDto.OriginNumber, createdTransaction.OriginNumber);
            var context = new UPBankAccountsContext(_options);
            Assert.Equal(1, await context.Transaction.CountAsync());
        }
    }
}
