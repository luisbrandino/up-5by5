namespace UPBank.Accounts.Api.Customer.Abstract
{
    public interface ICustomerApi
    {
        public Task<IEnumerable<Models.Customer>> Get();

        public Task<Models.Customer?> Get(string cpf);
    }
}
