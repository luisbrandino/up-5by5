using Microsoft.EntityFrameworkCore;
using UPBank.Agencies.Controllers;
using UPBank.Agencies.Data;
using UPBank.Models;

namespace UPBank.Tests.Agencies
{
    public class AgenciesTest
    {
        private DbContextOptions<UPBankAgenciesContext> options;

        public void InitializeDatabase()
        {
            options = new DbContextOptionsBuilder<UPBankAgenciesContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new UPBankAgenciesContext(options))
            {
                context.Agency.Add(new Agency
                {
                    Number = "123",
                    Cnpj = "12345678901234",
                    Restriction = false,
                    AddressZipcode = "12345678"
                });

                context.Agency.Add(new Agency
                {
                    Number = "456",
                    Cnpj = "12142678901434",
                    Restriction = false,
                    AddressZipcode = "12365678"
                });

                context.Agency.Add(new Agency
                {
                    Number = "789",
                    Cnpj = "947295736125",
                    Restriction = true,
                    AddressZipcode = "06147678"
                });
                context.SaveChanges();
            }
        }

        [Fact]
        public void GetAgencies()
        {
            InitializeDatabase();

            using (var context = new UPBankAgenciesContext(options))
            {
                var controller = new AgenciesController(context);
                var result = controller.GetAgency().Result;

                Assert.Equal(3, result.Value.Count());
            }
        }

        [Fact]
        public void GetAgency()
        {
            InitializeDatabase();

            using (var context = new UPBankAgenciesContext(options))
            {
                var controller = new AgenciesController(context);
                var result = controller.GetAgency("123").Result;

                Assert.Equal("123", result.Value.Number);
            }
        }

        [Fact]
        public void PutAgency()
        {
            InitializeDatabase();

            using (var context = new UPBankAgenciesContext(options))
            {
                var controller = new AgenciesController(context);
                var result = controller.GetAgency("123").Result;

                var agency = result.Value;
                agency.Cnpj = "12345678901234";
                agency.Restriction = true;

                _ = controller.PutAgency("123", agency);

                var resultUpdated = controller.GetAgency("123").Result;

                Assert.True(resultUpdated.Value.Restriction);
            }
        }

        [Fact]
        public async void PostAgency()
        {
            InitializeDatabase();

            using (var context = new UPBankAgenciesContext(options))
            {
                var controller = new AgenciesController(context);

                var invalidAgency = new Agency
                {
                    Number = "101",
                    Cnpj = "12345678901234",
                    Restriction = false,
                    AddressZipcode = "12345678"
                };

                var validAgency = new Agency
                {
                    Number = "202",
                    Cnpj = "64478045000159",
                    Restriction = false,
                    AddressZipcode = "12345678"
                };

                await controller.PostAgency(invalidAgency);
                await controller.PostAgency(validAgency);

                var invalidResult = controller.GetAgency("101").Result;
                var validResult = controller.GetAgency("202").Result;

                Assert.Null(invalidResult.Value);
                Assert.True(validResult.Value.Cnpj == "64478045000159");
            }
        }

        [Fact]
        public void DeleteAgency()
        {
            InitializeDatabase();

            using (var context = new UPBankAgenciesContext(options))
            {
                var controller = new AgenciesController(context);

                _ = controller.DeleteAgency("123");

                var result = controller.GetAgency("123").Result;

                Assert.Null(result.Value);
            }
        }
    }
}