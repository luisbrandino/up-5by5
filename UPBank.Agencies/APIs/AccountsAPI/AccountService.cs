using Newtonsoft.Json;
using UPBank.Agencies.APIs.AccountsAPI.Interface;
using UPBank.Agencies.APIs.Utils;
using UPBank.Enums;
using UPBank.Models;

namespace UPBank.Agencies.APIs.AccountsAPI
{
    public class AccountService : IAccountService
    {
        private readonly string _Account = "https://localhost:7193/api/Accounts/";

        public async Task<IEnumerable<Account>> GetRestrictedAccounts(string agencyNumber)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(_Account + "agency/" + agencyNumber + "/restricted");

            return await ApiUtils<IEnumerable<Account>>.GetObjectFromResponse(response) ?? new List<Account>();
        }

        public async Task<IEnumerable<Account>> GetAccountsByProfile(string agencyNumber, EProfile profile)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(_Account + "agency/" + agencyNumber + "/profile/" + profile);

            return await ApiUtils<IEnumerable<Account>>.GetObjectFromResponse(response) ?? new List<Account>();
        }

        public async Task<IEnumerable<Account>> GetAccountsWithActiveOverdraft(string agencyNumber)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(_Account + "agency/" + agencyNumber + "/activeloans");

            return await ApiUtils<IEnumerable<Account>>.GetObjectFromResponse(response) ?? new List<Account>();
        }
    }
}