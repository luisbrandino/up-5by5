using UPBank.Accounts.Specifications.Abstract;
using UPBank.Models;

namespace UPBank.Accounts.Specifications
{
    public class AccountUpdatingValidation : IAccountValidation
    {
        public void Validate(Account account)
        {
            if (account.Overdraft <= 0)
                throw new Exception("Cheque especial tem que ser positivo");
        }
    }
}
