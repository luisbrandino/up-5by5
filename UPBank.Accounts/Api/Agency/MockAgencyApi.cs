using UPBank.Accounts.Api.Agency.Abstract;

namespace UPBank.Accounts.Api.Agency
{
    public class MockAgencyApi : IAgencyApi
    {
        private readonly List<Models.Agency> _agencies = new List<Models.Agency>()
        {
            new Models.Agency()
            {
                Number = "10001",
                Address = null,
                AddressZipcode = null,
                Cnpj = "101.202.2021/22",
                Employees = new(),
                Restriction = false,
            },
            new Models.Agency()
            {
                Number = "20002",
                Address = null,
                AddressZipcode = null,
                Cnpj = "202.202.2055/22",
                Employees = new(),
                Restriction = true,
            },
            new Models.Agency()
            {
                Number = "30045",
                Address = null,
                AddressZipcode = null,
                Cnpj = "303.302.2011/23",
                Employees = new(),
                Restriction = false,
            },
        };

        public async Task<IEnumerable<Models.Agency>> Get()
        {
            return await Task.FromResult(_agencies);
        }

        public async Task<Models.Agency?> Get(string number)
        {
            return await Task.FromResult(_agencies.FirstOrDefault(a => a.Number == number));
        }
    }
}
