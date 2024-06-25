using UPBank.Accounts.Specifications.Abstract;
using UPBank.Models;

namespace UPBank.Accounts.Specifications
{
    public class TransactionCreationValidation : ITransactionValidation
    {
        public void Validate(Transaction transaction)
        {
            if (transaction.Value <= 0)
                throw new Exception("Valor da transação tem que ser positivo");

            if (transaction.Origin == null)
                throw new Exception("Conta de origem não cadastrada");
        }
    }
}
