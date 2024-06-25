using UPBank.Accounts.Api.Agency.Abstract;
using UPBank.Accounts.Api.Customer.Abstract;
using UPBank.Accounts.Data;
using UPBank.Accounts.Specifications;
using UPBank.DTOs;
using UPBank.Models;

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

            new AccountCreationValidation().Validate(account);

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

        public async Task<Account?> GetRaw(string number)
        {
            return await _context.Account.FindAsync(number);
        }

    }
}
