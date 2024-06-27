using Microsoft.EntityFrameworkCore;
using UPBank.Agencies.APIs.AccountsAPI;
using UPBank.Agencies.APIs.AddressesAPI;
using UPBank.Agencies.APIs.EmployeesAPI;
using UPBank.Agencies.Controllers;
using UPBank.Agencies.Data;
using UPBank.Agencies.Services;
using UPBank.DTOs;
using UPBank.Enums;
using UPBank.Models;

namespace UPBank.Tests.Agencies
{
    public class AgenciesControllerTest
    {
        private DbContextOptions<UPBankAgenciesContext> options;
        private AgenciesController controller;

        private AgenciesController GenerateController(UPBankAgenciesContext context)
        {
            return new(
                context,
                new AgencyService(
                    new MockEmployeeService(),
                    new MockAddressService(),
                    new MockAccountService()
                ));
        }

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
                    AddressZipcode = "22222-222",
                });

                context.Agency.Add(new Agency
                {
                    Number = "456",
                    Cnpj = "12142678901434",
                    Restriction = false,
                    AddressZipcode = "33333-333",
                });

                context.Agency.Add(new Agency
                {
                    Number = "789",
                    Cnpj = "947295736125",
                    Restriction = true,
                    AddressZipcode = "11111-111",
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
                var controller = GenerateController(context);
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
                var controller = GenerateController(context);
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
                var controller = GenerateController(context);
                var result = controller.GetAgency("123").Result;

                var agency = result.Value;

                agency.Cnpj = "12345678901234";
                agency.Restriction = true;

                var dto = new AgencyPutDTO()
                {
                    Number = agency.Number,
                    Cnpj = agency.Cnpj,
                    Restriction = agency.Restriction,
                    AddressZipcode = agency.Address.Zipcode
                };

                _ = controller.PutAgency(dto);

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
                var controller = GenerateController(context);

                var invalidAgency = new AgencyDTO
                {
                    Number = "101",
                    Cnpj = "12345678901234",
                    Manager = new AgencyCreationEmployeeDTO
                    { 
                        Name = "Manager", 
                        Manager = true 
                    },
                    Address = new AddressDTO 
                    { 
                        Zipcode = "11111-111",
                        Number = 1,
                        Complement = "Home" 
                    }
                };

                var validAgency = new AgencyDTO
                {
                    Number = "202",
                    Cnpj = "64478045000159",
                    Address = new AddressDTO
                    {
                        Zipcode = "11111-111",
                        Number = 1,
                        Complement = "Home"
                    },
                    Manager = new AgencyCreationEmployeeDTO
                    {
                        Name = "Manager",
                        Manager = true
                    }
                };

                await controller.PostAgency(invalidAgency);
                await controller.PostAgency(validAgency);

                var invalidResult = controller.GetAgency("101").Result;
                var validResult = controller.GetAgency("202").Result;

                Assert.Null(invalidResult.Value);
                Assert.True(validResult.Value.Cnpj == "64478045000159");
                Assert.Equal("Cidade 1", validResult.Value.Address.City);
            }
        }

        [Fact]
        public void DeleteAgency()
        {
            InitializeDatabase();

            using (var context = new UPBankAgenciesContext(options))
            {
                var controller = GenerateController(context);

                var ac = controller.DeleteAgency("123");

                var result = controller.GetAgency("123").Result;

                var deleted = context.Deleted.First(agency => agency.Number == "123");

                Assert.Null(result.Value);
                Assert.True(deleted.Cnpj == "12345678901234");
            }
        }

        [Fact]
        public void PatchAgency()
        {
            InitializeDatabase();

            using (var context = new UPBankAgenciesContext(options))
            {
                var controller = GenerateController(context);

                _ = controller.PatchAgencyRestriction("123");

                var result = controller.GetAgency("123").Result;

                Assert.True(result.Value.Restriction);
            }
        }

        [Fact]
        public void GetRestrictedAccounts()
        {
            InitializeDatabase();

            using (var context = new UPBankAgenciesContext(options))
            {
                var controller = GenerateController(context);

                var result = controller.GetRestrictedAccounts("123").Result;

                Assert.Equal(2, result.Value.Count());
            }
        }

        [Fact]
        public void GetAccountsByProfile()
        {
            InitializeDatabase();

            using (var context = new UPBankAgenciesContext(options))
            {
                var controller = GenerateController(context);

                var result = controller.GetAccountsByProfile("123", EProfile.University).Result;

                Assert.Equal(2, result.Value.Count());
            }
        }

        [Fact]
        public void GetAccountsWithActiveOverdraft()
        {
            InitializeDatabase();

            using (var context = new UPBankAgenciesContext(options))
            {
                var controller = GenerateController(context);

                var result = controller.GetAccountsWithActiveOverdraft("123").Result;

                Assert.Equal(2, result.Value.Count());
            }
        }
    }
}