using UPBank.Accounts.Specifications.Abstract;
using UPBank.Models;

namespace UPBank.Accounts.Specifications
{
    public class BaseTransactionCreationValidation : ITransactionValidation
    {
        public virtual void Validate(Transaction transaction)
        {
            if (transaction.Value <= 0)
                throw new Exception("Valor da transação tem que ser positivo");

            if (transaction.Origin == null)
                throw new Exception("Conta de origem não cadastrada");
        }
    }

    public class DebitTransactionCreationValidation : BaseTransactionCreationValidation
    {
        public override void Validate(Transaction transaction)
        {
            base.Validate(transaction);

            if (transaction.Origin.Balance < transaction.Value)
                throw new Exception("Conta de origem não possui saldo suficiente");
        }
    }

    public class TransferTransactionCreationValidation : DebitTransactionCreationValidation
    {
        public override void Validate(Transaction transaction)
        {
            base.Validate(transaction);

            if (transaction.Destiny == null)
                throw new Exception("Conta de destino não cadastrada");

            if (transaction.Origin.Number == transaction.Destiny.Number)
                throw new Exception("Conta de origem não pode ser a mesma que conta de destino");
        }
    }
}
