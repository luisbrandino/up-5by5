using Newtonsoft.Json;
using UPBank.Agencies.APIs.AccountsAPI.Interface;
using UPBank.Enums;
using UPBank.Models;

namespace UPBank.Agencies.APIs.AccountsAPI
{
    public class AccountService : IAccountService
    {
        private readonly string _Account = "https://localhost5001/api/Accounts/";

        public async Task<IEnumerable<Account>> GetRestrictedAccounts(string agencyNumber) => await Task.FromResult(GetAccountsFromAgency(agencyNumber).Result.Where(account => account.Restriction));

        public async Task<IEnumerable<Account>> GetAccountsByProfile(string agencyNumber, EProfile profile) => await Task.FromResult(GetAccountsFromAgency(agencyNumber).Result.Where(account => account.Profile == profile));

        public async Task<IEnumerable<Account>> GetAccountsWithActiveOverdraft(string agencyNumber) => await Task.FromResult(GetAccountsFromAgency(agencyNumber).Result.Where(account => account.Overdraft > 0));

        private async Task<IEnumerable<Account>> GetAccountsFromAgency(string agencyNumber)
        {
            var url = _Account + agencyNumber;
            List<Account> accounts = new List<Account>();

            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(json))
                    accounts = JsonConvert.DeserializeObject<IEnumerable<Account>>(json).ToList();
            }

            return accounts;
        }
    }
}