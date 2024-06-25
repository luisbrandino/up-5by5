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

            context.CreditCard.Add(new CreditCard
            {
                Number = 2020,
                CVV = "101",
                Brand = "Visa",
                Active = true,
                ExtractionDate = DateTime.Now,
                Holder = "Um cara ai",
                Limit = 1500
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

            context.CreditCard.Add(new CreditCard
            {
                Number = 1111,
                CVV = "131",
                Brand = "MasterCard",
                Active = false,
                ExtractionDate = DateTime.Now,
                Holder = "Dois cara ai",
                Limit = 1750
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
            Assert.Equal(3, await context.CreditCard.CountAsync());
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
        public async Task Post_NoClients_ReturnsBadRequest()
        {
            var controller = Make();

            var accountCreationDTO = new AccountCreationDTO
            {
                Overdraft = 2000,
                Profile = Enums.EProfile.Normal,
                AgencyNumber = "10001",
                Customers = new List<string> { }
            };

            var createdResult = await controller.Post(accountCreationDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(createdResult.Result);
            var badRequestMessage = badRequestResult.Value;
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("É necessário que a conta pertença à pelo menos um cliente", badRequestMessage);
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
            Assert.Equal(3, await context.CreditCard.CountAsync());
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
        public async Task Post_PaymentTransactionWithNegativeValue_ReturnsBadRequest()
        {
            var controller = Make();

            var transactionCreationDto = new TransactionCreationDTO
            {
                Type = Enums.EType.Payment,
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
        public async Task Post_DepositTransactionWithNegativeValue_ReturnsBadRequest()
        {
            var controller = Make();

            var transactionCreationDto = new TransactionCreationDTO
            {
                Type = Enums.EType.Deposit,
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
        public async Task Post_TransferTransactionWithNegativeValue_ReturnsBadRequest()
        {
            var controller = Make();

            var transactionCreationDto = new TransactionCreationDTO
            {
                Type = Enums.EType.Transfer,
                OriginNumber = "123456789",
                DestinyNumber = "505020",
                Value = -10
            };

            var createdResult = await controller.MakeTransaction(transactionCreationDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(createdResult.Result);
            var badRequestMessage = badRequestResult.Value;
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Valor da transação tem que ser positivo", badRequestMessage);
        }

        [Fact]
        public async Task Post_WithdrawTransactionWithNegativeValue_ReturnsBadRequest()
        {
            var controller = Make();

            var transactionCreationDto = new TransactionCreationDTO
            {
                Type = Enums.EType.Withdraw,
                OriginNumber = "123456789",
                DestinyNumber = "505020",
                Value = -10
            };

            var createdResult = await controller.MakeTransaction(transactionCreationDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(createdResult.Result);
            var badRequestMessage = badRequestResult.Value;
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Valor da transação tem que ser positivo", badRequestMessage);
        }

        [Fact]
        public async Task Post_LoanTransactionWithNegativeValue_ReturnsBadRequest()
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
        public async Task Post_TransferTransactionWithNoDestinyAccount_ReturnsBadRequest()
        {
            var controller = Make();

            var transactionCreationDto = new TransactionCreationDTO
            {
                Type = Enums.EType.Transfer,
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
        public async Task Post_TransferTransactionWithInvalidDestinyAccount_ReturnsBadRequest()
        {
            var controller = Make();

            var transactionCreationDto = new TransactionCreationDTO
            {
                Type = Enums.EType.Transfer,
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
        public async Task Post_TransferTransactionWithDuplicateOriginAndDestinyAccount_ReturnsBadRequest()
        {
            var controller = Make();

            var transactionCreationDto = new TransactionCreationDTO
            {
                Type = Enums.EType.Transfer,
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
        public async Task Post_TransferTransactionNotEnoughBalance_ReturnsBadRequest()
        {
            var controller = Make();

            var transactionCreationDto = new TransactionCreationDTO
            {
                Type = Enums.EType.Transfer,
                OriginNumber = "123456789",
                DestinyNumber = "505020",
                Value = 3000
            };

            var createdResult = await controller.MakeTransaction(transactionCreationDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(createdResult.Result);
            var badRequestMessage = badRequestResult.Value;
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Conta de origem não possui saldo suficiente", badRequestMessage);
        }

        [Fact]
        public async Task Post_WithdrawTransactionNotEnoughBalance_ReturnsBadRequest()
        {
            var controller = Make();

            var transactionCreationDto = new TransactionCreationDTO
            {
                Type = Enums.EType.Withdraw,
                OriginNumber = "123456789",
                Value = 3000
            };

            var createdResult = await controller.MakeTransaction(transactionCreationDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(createdResult.Result);
            var badRequestMessage = badRequestResult.Value;
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Conta de origem não possui saldo suficiente", badRequestMessage);
        }

        [Fact]
        public async Task Post_PaymentTransactionNotEnoughBalance_ReturnsBadRequest()
        {
            var controller = Make();

            var transactionCreationDto = new TransactionCreationDTO
            {
                Type = Enums.EType.Payment,
                OriginNumber = "123456789",
                Value = 3000
            };

            var createdResult = await controller.MakeTransaction(transactionCreationDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(createdResult.Result);
            var badRequestMessage = badRequestResult.Value;
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Conta de origem não possui saldo suficiente", badRequestMessage);
        }

        [Fact]
        public async Task Post_ValidTransferTransaction_ReturnsCreatedTransaction()
        {
            var controller = Make();

            var transactionCreationDto = new TransactionCreationDTO
            {
                Type = Enums.EType.Transfer,
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
            Assert.Equal(1000, (await context.Account.FindAsync("123456789")).Balance);
            Assert.Equal(2111, (await context.Account.FindAsync("505020")).Balance);
        }

        [Fact]
        public async Task Post_ValidPaymentTransaction_ReturnsCreatedTransaction()
        {
            var controller = Make();

            var transactionCreationDto = new TransactionCreationDTO
            {
                Type = Enums.EType.Payment,
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
            Assert.Equal(1000, (await context.Account.FindAsync("123456789")).Balance);
        }

        [Fact]
        public async Task Post_ValidDepositTransaction_ReturnsCreatedTransaction()
        {
            var controller = Make();

            var transactionCreationDto = new TransactionCreationDTO
            {
                Type = Enums.EType.Deposit,
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
            Assert.Equal(3000, (await context.Account.FindAsync("123456789")).Balance);
        }

        [Fact]
        public async Task Post_ValidWithdrawTransaction_ReturnsCreatedTransaction()
        {
            var controller = Make();

            var transactionCreationDto = new TransactionCreationDTO
            {
                Type = Enums.EType.Withdraw,
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
            Assert.Equal(1000, (await context.Account.FindAsync("123456789")).Balance);
        }

        [Fact]
        public async Task Post_ValidLoanTransaction_ReturnsCreatedTransaction()
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
            Assert.Equal(3000, (await context.Account.FindAsync("123456789")).Balance);
        }

        [Fact]
        public async Task Post_ActivateCreditCardAccountNotInDatabase_ReturnsBadRequest()
        {
            var controller = Make();

            var createdResult = await controller.ActivateCreditCard("130847981347");

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(createdResult.Result);
            var badRequestMessage = badRequestResult.Value;
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Conta não cadastrada", badRequestMessage);
        }

        [Fact]
        public async Task Post_ActivateCreditCardAlreadyActive_ReturnsBadRequest()
        {
            var controller = Make();

            var createdResult = await controller.ActivateCreditCard("123456789");

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(createdResult.Result);
            var badRequestMessage = badRequestResult.Value;
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Cartão de crédito já está ativado", badRequestMessage);
        }

        [Fact]
        public async Task Post_ActivateCreditCard_ReturnsCreditCard()
        {
            var controller = Make();

            var activatedCreditCardResult = await controller.ActivateCreditCard("505020");

            var creditCardActivated = Assert.IsType<CreditCard>(activatedCreditCardResult.Value);
            Assert.True(creditCardActivated.Active);
            var context = new UPBankAccountsContext(_options);
            Assert.True((await context.CreditCard.FindAsync((long) 1111)).Active);
        }

        [Fact]
        public async Task Get_AllAccounts_ReturnsAccounts()
        {
            var controller = Make();

            var accountsResult = await controller.GetAccount();

            var accounts = Assert.IsType<List<Account>>(accountsResult.Value);
            Assert.Equal(2, accounts.Count);
        }

        [Fact]
        public async Task Get_Account_ReturnsAccounts()
        {
            var controller = Make();

            var accountResult = await controller.GetAccount("123456789");

            var account = Assert.IsType<Account>(accountResult.Value);
            Assert.Equal("123456789", account.Number);
            Assert.Equal(2000, account.Balance);
        }

        [Fact]
        public async Task Get_AccountNotInDatabase_ReturnsNotFound()
        {
            var controller = Make();

            var accountResult = await controller.GetAccount("1232345678239");

            var notFoundResult = Assert.IsType<NotFoundResult>(accountResult.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}
