using UPBank.Models;

namespace UPBank.Accounts.Specifications.Abstract
{
    public interface ICreditCardValidation
    {
        public void Validate(CreditCard creditCard);
    }
}
