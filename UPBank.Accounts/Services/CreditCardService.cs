using UPBank.Models;

namespace UPBank.Accounts.Services
{
    public class CreditCardService
    {

        public async Task<CreditCard> Create()
        {
            return await Task.FromResult(new CreditCard());
        }

    }
}
