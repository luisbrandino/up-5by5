using Mongo2Go;
using MongoDB.Driver;
using UPBank.Addresses.Mongo.Repositories;
using UPBank.Models;

namespace UPBank.Tests.Addresses
{
    public class AddressRepositoryTest
    {
        private readonly MongoDbRunner _runner;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Address> _collection;
        private readonly AddressRepository _repository;

        public AddressRepositoryTest()
        {
            _runner = MongoDbRunner.Start();
            var client = new MongoClient(_runner.ConnectionString);
            _database = client.GetDatabase("UPBank");

            _collection = _database.GetCollection<Address>("Address");

            for (int i = 0; i < 3; i++)
                _collection.InsertOne(new Address
                {
                    Zipcode = $"{68970 + i}-{850 + i}",
                    Street = "Avenida José Lorenço 1299",
                    Neighborhood = "Centro",
                    City = "Lourenço",
                    State = "AP"
                });

            _repository = new AddressRepository(new UPBank.Addresses.Mongo.Settings.MongoDatabaseSettings()
            {
                ConnectionString = _runner.ConnectionString
            });
        }

        [Fact]
        public async Task FindAll_Addresses_ShouldHaveThreeItems()
        {
            var addresses = await _repository.Find();

            Assert.Equal(3, addresses.Count());
        }

        [Fact]
        public async Task Find_ValidZipcode_ShouldReturnAddress()
        {
            var address = await _repository.Find("68971851");

            Assert.IsType<Address>(address);
            Assert.Equal("68971-851", address.Zipcode);
        }

        [Fact]
        public async Task Find_InvalidZipcode_ShouldReturnNull()
        {
            var address = await _repository.Find("68971822");

            Assert.Null(address);
        }

        [Fact]
        public async Task Insert_Address_ShouldInsertInDatabase()
        {
            var address = new Address
            {
                Zipcode = "15990-840",
                Street = "Rua cesário motta",
                Neighborhood = "Bairro",
                State = "SP",
                City = "Matão",
                Complement = "Casa A",
                Number = 8
            };

            _repository.Insert(address);

            var insertedAddress = await (await _collection.FindAsync(address => address.Zipcode == "15990-840")).FirstOrDefaultAsync();

            Assert.NotNull(insertedAddress);
            Assert.Equal(insertedAddress.Zipcode, address.Zipcode);
            Assert.Equal(4, await _collection.CountAsync(_ => true));
        }
    }
}