using MongoDB.Driver;
using System.Text.RegularExpressions;
using UPBank.Addresses.Mongo.Settings;
using UPBank.Models;

namespace UPBank.Addresses.Mongo.Repositories
{
    public class AddressRepository
    {
        private readonly IMongoCollection<Address> _collection;

        public AddressRepository(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<Address>(settings.AddressCollectionName);
        }

        public void Insert(Address address) => _collection.InsertOne(address);

        public async Task<Address?> Find(string zipcode)
        {
            zipcode = ParseZipcode(zipcode);
            return await (await _collection.FindAsync(address => address.Zipcode == zipcode)).FirstOrDefaultAsync();
        }

        public async Task<List<Address>> Find()
        {
            return await (await _collection.FindAsync(_ => true)).ToListAsync();
        }

        private string ParseZipcode(string zipcode)
        {
            if (Regex.IsMatch(zipcode, @"^\d{5}-\d{3}$"))
                return zipcode;

            return $"{zipcode.Substring(0, 5)}-{zipcode.Substring(5)}";
        }
    }
}
