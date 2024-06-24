using UPBank.Models;

namespace UPBank.Accounts.Specifications.Abstract
{
    public interface IAccountValidation
    {
        public void Validate(Account account);
    }
}
