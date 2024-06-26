using UPBank.Accounts.Api.Customer.Abstract;

namespace UPBank.Accounts.Api.Customer
{
    public class UPBankCostumerApi : ICustomerApi
    {
        private readonly IConsumer _consumer;
        private readonly string _url = "https://localhost:7136/api/customers";

        public UPBankCostumerApi(IConsumer consumer)
        {
            _consumer = consumer;
        }

        public async Task<IEnumerable<Models.Customer>> Get()
        {
            var customers = await _consumer.Get<List<Models.Customer>>(_url);

            if (customers == null)
                return new List<Models.Customer>();

            return customers;
        }

        public async Task<Models.Customer?> Get(string cpf)
        {
            return await _consumer.Get<Models.Customer>(_url + $"/{cpf}");
        }
    }
}
