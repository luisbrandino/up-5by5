using UPBank.Models;

namespace UPBank.Accounts.Specifications.Abstract
{
    public interface ITransactionValidation
    {
        public void Validate(Transaction transaction);
    }
}
