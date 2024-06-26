using UPBank.Accounts.Operations.Abstract;
using UPBank.Models;

namespace UPBank.Accounts.Operations
{
    public class TransferTransactionOperation : ITransactionOperation
    {
        public void Execute(Transaction transaction)
        {
            transaction.Origin.Balance -= transaction.Value;
            transaction.Destiny.Balance += transaction.Value;
        }
    }
}
