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
                var agency = new Agency
                {
                    Number = "101",
                    Cnpj = "12345678901234",
                    Restriction = false,
                    AddressZipcode = "12345678"
                };

                await controller.PostAgency(agency);

                var result = controller.GetAgency("101").Result;

                Assert.Equal("101", result.Value.Number);
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