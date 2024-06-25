using UPBank.Accounts.Operations.Abstract;
using UPBank.Models;

namespace UPBank.Accounts.Operations
{
    public class DebitTransactionOperation : ITransactionOperation
    {
        public void Execute(Transaction transaction)
        {
            transaction.Origin.Balance -= transaction.Value;
        }
    }
}
