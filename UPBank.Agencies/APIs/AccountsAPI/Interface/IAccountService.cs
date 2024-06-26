using UPBank.Enums;
using UPBank.Models;

namespace UPBank.Agencies.APIs.AccountsAPI.Interface
{
    public interface IAccountService
    {
        public Task<IEnumerable<Account>> GetRestrictedAccounts(string agencyNumber);

        public Task<IEnumerable<Account>> GetAccountsByProfile(string agencyNumber, EProfile profile);

        public Task<IEnumerable<Account>> GetAccountsWithActiveOverdraft(string agencyNumber);
    }
}