using UPBank.Accounts.Api.Agency.Abstract;

namespace UPBank.Accounts.Api.Agency
{
    public class UPBankAgencyApi : IAgencyApi
    {
        private readonly IConsumer _consumer;
        private readonly string _url = "";

        public UPBankAgencyApi(IConsumer consumer)
        {
            _consumer = consumer;
        }

        public async Task<IEnumerable<Models.Agency>> Get()
        {
            var agencies = await _consumer.Get<List<Models.Agency>>(_url);

            if (agencies == null)
                return new List<Models.Agency>();

            return agencies;
        }

        public async Task<Models.Agency?> Get(string number)
        {
            return await _consumer.Get<Models.Agency>(_url + $"/{number}");
        }
    }
}
