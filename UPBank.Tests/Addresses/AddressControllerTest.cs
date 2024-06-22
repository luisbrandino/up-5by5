using Microsoft.AspNetCore.Mvc;
using Mongo2Go;
using MongoDB.Driver;
using UPBank.Addresses.Controllers;
using UPBank.Addresses.Mongo.Repositories;
using UPBank.Addresses.Mongo.Settings;
using UPBank.Addresses.PostalServices;
using UPBank.DTOs;
using UPBank.Models;

namespace UPBank.Tests.Addresses
{
    public class AddressControllerTest
    {
        private readonly MongoDbRunner _runner;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Address> _collection;
        private readonly AddressRepository _repository;
        private readonly UPBank.Addresses.Mongo.Settings.MongoDatabaseSettings _settings;

        public AddressControllerTest()
        {
            _runner = MongoDbRunner.Start();

            _settings = new()
            {
                ConnectionString = _runner.ConnectionString
            };

            var client = new MongoClient(_runner.ConnectionString);
            _database = client.GetDatabase(_settings.DatabaseName);
            _collection = _database.GetCollection<Address>(_settings.AddressCollectionName);

            _collection.InsertOne(new Address
            {
                City = "Matão",
                Complement = "Casa B",
                Number = 2,
                State = "SP",
                Street = "Rua Sinharia Frota",
                Zipcode = "15990-220",
                Neighborhood = "Centro"
            });

            _repository = new AddressRepository(_settings);
        }

        [Fact]
        public async Task Get_ReturnsAllAddresses()
        {
            var service = new ViaCepService();
            var controller = new AddressesController(_repository, service);

            var result = await controller.Get();

            if (result.Result is OkObjectResult okObject)
            {
                var addresses = Assert.IsAssignableFrom<IEnumerable<Address>>(okObject.Value);
                Assert.Equal(200, okObject.StatusCode);
                Assert.IsType<List<Address>>(addresses);
                Assert.Equal(1, addresses.Count());
            }
        }

        [Fact]
        public async Task Get_ValidZipcode_ReturnsAddress()
        {
            var service = new ViaCepService();
            var controller = new AddressesController(_repository, service);

            var result = await controller.Get("15990220");
            var okObject = result.Result as OkObjectResult;

            var address = Assert.IsAssignableFrom<Address>(okObject.Value);
            Assert.Equal(200, okObject.StatusCode);
            Assert.IsType<Address>(address);
            Assert.Equal("15990-220", address.Zipcode);
        }

        [Fact]
        public async Task Get_InvalidZipcode_ReturnsNotFound()
        {
            var service = new ViaCepService();
            var controller = new AddressesController(_repository, service);

            var result = await controller.Get("159902220");

            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Post_ValidAddress_ReturnsCreatedResponse()
        {
            var service = new ViaCepService();
            var controller = new AddressesController(_repository, service);

            var addressDto = new AddressDTO
            {
                Complement = "Apto 101",
                Number = 2,
                Zipcode = "15990-840"
            };

            var createdResult = await controller.Post(addressDto);

            var createdAddress = Assert.IsType<Address>(createdResult.Value);
            Assert.Equal(addressDto.Complement, createdAddress.Complement);
            Assert.Equal(addressDto.Number, createdAddress.Number);
        }

        [Fact]
        public async Task Post_ValidAddressExistsInDatabase_ReturnsConflict()
        {
            var service = new ViaCepService();
            var controller = new AddressesController(_repository, service);

            var addressDto = new AddressDTO
            {
                Complement = "Apto 101",
                Number = 2,
                Zipcode = "15990-220"
            };

            var createdResult = await controller.Post(addressDto);

            var conflictResult = Assert.IsType<ConflictObjectResult>(createdResult.Result);
            Assert.Equal(409, conflictResult.StatusCode);
        }

        [Fact]
        public async Task Post_InvalidAddress_ReturnsBadRequest()
        {
            var service = new ViaCepService();
            var controller = new AddressesController(_repository, service);

            var addressDto = new AddressDTO
            {
                Complement = "Apto 101",
                Number = 2,
                Zipcode = "15990-2220"
            };

            var createdResult = await controller.Post(addressDto);

            var badRequestResult = Assert.IsType<BadRequestResult>(createdResult.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
    }
}
