using UPBank.Accounts.Specifications.Abstract;
using UPBank.Models;

namespace UPBank.Accounts.Specifications
{
    public class AccountCreationValidation : IAccountValidation
    {
        public void Validate(Account account)
        {
            // o ideal seria separar cada uma dessas regras de negócio em sua propria classe,
            // mas depois eu avalio se vale a pena
            if (account.Agency == null)
                throw new Exception("Agência não cadastrada");

            if (account.Customers.Any(customer => customer == null))
                throw new Exception("Cliente não cadastrado");

            if (account.Customers.Count > 2)
                throw new Exception("Uma conta pode ter no máximo dois clientes");

            if (account.Customers.Count <= 0)
                throw new Exception("É necessário que a conta pertence à pelo menos um cliente");

            if (account.Customers.Count != account.Customers.Distinct().Count())
                throw new Exception("Não pode haver clientes repetidos");

            Customer firstCustomer = account.Customers.First();

            int age = DateTime.Today.Year - firstCustomer.BirthDate.Year;

            if (firstCustomer.BirthDate > DateTime.Today.AddYears(-age))
                age--;

            if (age <= 18)
                throw new Exception("O primeiro cliente da conta deve ser maior de idade");
        }
    }
}
