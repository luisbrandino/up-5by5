using UPBank.Accounts.Specifications.Abstract;
using UPBank.Models;

namespace UPBank.Accounts.Specifications
{
    public class CreditCardActivationValidation : ICreditCardValidation
    {
        public void Validate(CreditCard creditCard)
        {
            if (creditCard == null)
                throw new Exception("Conta não cadastrada");

            if (creditCard.Active)
                throw new Exception("Cartão de crédito já está ativado");
        }
    }
}
