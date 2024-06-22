using Newtonsoft.Json;
using UPBank.Enums;
using UPBank.Models;

namespace UPBank.Agencies.Services
{
    public class AgencyService
    {
        private readonly string _Account = "https://localhost:####/api/Accounts/";
    
        public async Task<IEnumerable<Account>> GetAccountsFromAgency(string agencyNumber)
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

        public async Task<IEnumerable<Account>> GetAccountsByProfile(EProfile profile)
        {
            var url = _Account + "####" + profile;
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

        public async Task<IEnumerable<Account>> GetAccountsWithActiveOverdraft()
        {
            var url = _Account + "WithActiveOverdraft";
            List<Account> accounts = new List<Account>();

            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(json))
                    accounts = JsonConvert.DeserializeObject<IEnumerable<Account>>(json).ToList();
            }
        }
    }
}