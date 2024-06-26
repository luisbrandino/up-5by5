using Humanizer;
using UPBank.Accounts.Data;
using UPBank.Accounts.Services;
using UPBank.Accounts.Specifications.Abstract;
using UPBank.Models;

namespace UPBank.Accounts.Specifications
{
    public class AccountCreationValidation : IAccountValidation
    {
        private readonly AccountService _service;

        public AccountCreationValidation(AccountService service)
        {
            _service = service;
        }

        public void Validate(Account account)
        {
            // o ideal seria separar cada uma dessas regras de negócio em sua propria classe,
            // mas depois eu avalio se vale a pena
            if (_service.Exists(account.Number))
                throw new Exception("Número de conta já existe");

            if (account.Agency == null)
                throw new Exception("Agência não cadastrada");

            if (account.Customers.Any(customer => customer == null))
                throw new Exception("Cliente não cadastrado");

            if (account.Customers.Count > 2)
                throw new Exception("Uma conta pode ter no máximo dois clientes");

            if (account.Customers.Count <= 0)
                throw new Exception("É necessário que a conta pertença à pelo menos um cliente");

            if (account.Customers.Count != account.Customers.Distinct().Count())
                throw new Exception("Não pode haver clientes repetidos");

            Customer firstCustomer = account.Customers.First();

            int age = DateTime.Today.Year - firstCustomer.BirthDate.Year;

            if (firstCustomer.BirthDate > DateTime.Today.AddYears(-age))
                age--;

            if (age <= 18)
                throw new Exception("O primeiro cliente da conta deve ser maior de idade");

            if (_service.CustomerHasAccount(firstCustomer.Cpf))
                throw new Exception("Cliente já possui conta");
        }
    }
}
