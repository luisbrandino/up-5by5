using UPBank.Agencies.APIs.AccountsAPI.Interface;
using UPBank.Enums;
using UPBank.Models;

namespace UPBank.Agencies.APIs.AccountsAPI
{
    public class MockAccountService : IAccountService
    {
        private readonly List<Account> _accounts;

        public MockAccountService()
        {
            _accounts = new List<Account>
            {
                new Account
                {
                    Number = "123456",
                    Restriction = false,
                    Overdraft = 0,
                    Profile = EProfile.Normal,
                    CreationDate = DateTime.Now,
                    Balance = 1000,
                    AgencyNumber = "123"
                },
                new Account
                {
                    Number = "654321",
                    Restriction = true,
                    Overdraft = 100,
                    Profile = EProfile.University,
                    CreationDate = DateTime.Now,
                    Balance = 5000,
                    AgencyNumber = "123"
                },
                new Account
                {
                    Number = "987654",
                    Restriction = false,
                    Overdraft = 0,
                    Profile = EProfile.University,
                    CreationDate = DateTime.Now,
                    Balance = 2000,
                    AgencyNumber = "123"
                },
                new Account
                {
                    Number = "456789",
                    Restriction = true,
                    Overdraft = 50,
                    Profile = EProfile.Vip,
                    CreationDate = DateTime.Now,
                    Balance = 3000,
                    AgencyNumber = "123"
                }
            };
        }

        public async Task<IEnumerable<Account>> GetRestrictedAccounts(string agencyNumber) => await Task.FromResult(_accounts.ToList().Where(a => a.Restriction && a.AgencyNumber == agencyNumber));

        public async Task<IEnumerable<Account>> GetAccountsByProfile(string agencyNumber, EProfile profile) => await Task.FromResult(_accounts.ToList().Where(a => a.Profile == profile && a.AgencyNumber == agencyNumber));

        public async Task<IEnumerable<Account>> GetAccountsWithActiveOverdraft(string agencyNumber) => await Task.FromResult(_accounts.ToList().Where(a => a.Overdraft > 0 && a.AgencyNumber == agencyNumber));
    }
}