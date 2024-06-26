namespace UPBank.Accounts.Api.Agency.Abstract
{
    public interface IAgencyApi
    {
        public Task<IEnumerable<Models.Agency>> Get();
        public Task<Models.Agency?> Get(string number);
    }
}
