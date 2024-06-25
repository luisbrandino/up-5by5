using UPBank.Models;

namespace UPBank.Accounts.Operations.Abstract
{
    public interface ITransactionOperation
    {
        public void Execute(Transaction transaction);
    }
}
