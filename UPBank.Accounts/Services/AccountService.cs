using Microsoft.EntityFrameworkCore;
using UPBank.Accounts.Api.Agency.Abstract;
using UPBank.Accounts.Api.Customer.Abstract;
using UPBank.Accounts.Data;
using UPBank.Accounts.DTO;
using UPBank.Accounts.Specifications;
using UPBank.DTOs;
using UPBank.Enums;
using UPBank.Models;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace UPBank.Accounts.Services
{
    public class AccountService
    {
        private readonly UPBankAccountsContext _context;
        private readonly CreditCardService _creditCardService;
        private readonly ICustomerApi _customer;
        private readonly IAgencyApi _agency;

        public AccountService(UPBankAccountsContext context, CreditCardService creditCardService, ICustomerApi customer, IAgencyApi agency)
        {
            _creditCardService = creditCardService;
            _context = context;
            _customer = customer;
            _agency = agency;
        }

        private string GenerateAccountNumber() => new Random().Next(1, 100000).ToString("000000");

        private string GenerateSavingAccountsDigits() => new Random().Next(1, 99).ToString("00");

        private async Task<Account> MakeFromRequestedAccount(AccountCreationDTO requestedAccount)
        {
            Agency? agency = await _agency.Get(requestedAccount.AgencyNumber);

            var tasks = requestedAccount.Customers.Select(cpf => _customer.Get(cpf));
            List<Customer?> customers = (await Task.WhenAll(tasks)).ToList();

            Account account = new Account
            {
                Number = GenerateAccountNumber(),
                Overdraft = 500,
                Agency = agency,
                AgencyNumber = agency?.Number,
                Customers = customers,
                Profile = requestedAccount.Profile,
                CreationDate = DateTime.Now,
                Balance = 2000,
                Restriction = true,
            };

            account.SavingsAccount = requestedAccount.IsSavingsAccount ? $"{account.Number}-{GenerateSavingAccountsDigits()}" : null;

            return account;
        }

        public async Task<Account> Create(AccountCreationDTO requestedAccount)
        {
            Account account = await MakeFromRequestedAccount(requestedAccount);

            new AccountCreationValidation(this).Validate(account);

            account.CreditCard = await _creditCardService.Create(account.Customers.First().Name);
            account.CreditCardNumber = account.CreditCard.Number;

            account.Customers.ForEach(customer =>
            {
                AccountCustomer accountCustomer = new AccountCustomer
                {
                    AccountNumber = account.Number,
                    CustomerCpf = customer.Cpf
                };

                _context.AccountCustomer.Add(accountCustomer);
            });

            _context.Add(account);
            await _context.SaveChangesAsync();

            return account;
        }

        public async Task<CreditCard> ActivateCreditCard(string accountNumber)
        {
            return await _creditCardService.Activate(await GetCreditCard(accountNumber));
        }

        public async Task<CreditCard?> GetCreditCard(string accountNumber)
        {
            var account = await GetRaw(accountNumber);

            if (account == null)
                return null;

            return await _context.CreditCard.FirstOrDefaultAsync(creditCard => creditCard.Number == account.CreditCardNumber);
        }

        public async Task<Account?> GetRaw(string number)
        {
            return await _context.Account.FindAsync(number);
        }

        public async Task<Account?> Get(string number)
        {
            var account = await _context.Account.FindAsync(number);

            if (account == null)
                return null;

            var agency = await _agency.Get(account.AgencyNumber);

            var customersInAccount = await _context.AccountCustomer.Where(ac => ac.AccountNumber == account.Number).ToListAsync();
            var customers = new List<Customer>();

            foreach (var customer in customersInAccount)
                customers.Add(await _customer.Get(customer.CustomerCpf));

            account.Agency = agency;
            account.Customers = customers;
            return account;
        }

        public async Task<IEnumerable<Account>> Get()
        {
            var accounts = await _context.Account.ToListAsync();

            foreach (var account in accounts)
            {
                Agency agency = await _agency.Get(account.AgencyNumber);
                var customersinaccount = await _context.AccountCustomer.Where(ac => ac.AccountNumber == account.Number).ToListAsync();
                var customers = new List<Customer>();

                foreach (var customer in customersinaccount)
                    customers.Add(await _customer.Get(customer.CustomerCpf));

                account.Customers = customers;
                account.Agency = agency;
            }

            return accounts;
        }

        public async Task<IEnumerable<Transaction>?> GetStatement(string number)
        {
            var account = await Get(number);

            if (account == null)
                return null;

            var transactions = await _context.Transaction
                .Where(transaction => transaction.OriginNumber == account.Number)
                .ToListAsync();

            return transactions;
        }

        public async Task<IEnumerable<Transaction>?> GetTransactionsByType(string accountNumber, EType transactionType)
        {
            var account = await Get(accountNumber);

            if (account == null)
                return null;

            var transactions = await _context.Transaction.Where(
                transaction => transaction.Type == transactionType && 
                transaction.OriginNumber == account.Number)
                .ToListAsync();

            transactions.Select(transaction => transaction.Origin = account);

            return transactions;
        }

        public async Task<DeletedAccount?> Delete(string number)
        {
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var account = await _context.Account.FindAsync(number);

                if (account == null)
                    return null;

                var deletedAccount = CreateDeletedAccountFromAccount(account);
                _context.DeletedAccount.Add(deletedAccount);
                _context.Account.Remove(account);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return deletedAccount;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public static DeletedAccount CreateDeletedAccountFromAccount(Account account)
        {
            return new DeletedAccount
            {
                Number = account.Number,
                Restriction = account.Restriction,
                Overdraft = account.Overdraft,
                Profile = account.Profile,
                CreationDate = account.CreationDate,
                Balance = account.Balance,
                SavingsAccount = account.SavingsAccount,
                CreditCardNumber = account.CreditCardNumber,
                AgencyNumber = account.AgencyNumber
            };
        }

        public bool Exists(string number) => (_context.Account?.Any(a => a.Number == number)).GetValueOrDefault();
    }
}
