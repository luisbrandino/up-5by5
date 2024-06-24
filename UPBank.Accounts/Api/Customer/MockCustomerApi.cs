﻿using UPBank.Accounts.Api.Customer.Abstract;

namespace UPBank.Accounts.Api.Customer
{
    public class MockCustomerApi : ICustomerApi
    {
        private readonly List<Models.Customer> _customers = new List<Models.Customer>
        {
            new Models.Customer()
            {
                Name = "Test",
                BirthDate = DateTime.Now.AddYears(-20),
                Cpf = "000.000.000-01",
                Email = "test@test.com",
                Gender = 'M',
                Phone = "16999998888",
                Restriction = true,
                Salary = 1500.99,
                AddressZipcode = null,
                Address = null,
            },
            new Models.Customer()
            {
                Name = "Test2",
                BirthDate = DateTime.Now,
                Cpf = "000.000.000-02",
                Email = "test2@test2.com",
                Gender = 'F',
                Phone = "16999998887",
                Restriction = false,
                Salary = 700.11,
                AddressZipcode = null,
                Address = null,
            },
            new Models.Customer()
            {
                Name = "Test3",
                BirthDate = DateTime.Now,
                Cpf = "000.000.000-03",
                Email = "test3@test3.com",
                Gender = 'M',
                Phone = "16999998881",
                Restriction = false,
                Salary = 2001.99,
                AddressZipcode = null,
                Address = null,
            },
        };

        public async Task<IEnumerable<Models.Customer>> Get()
        {
            return await Task.FromResult(_customers);
        }

        public async Task<Models.Customer?> Get(string cpf)
        {
            return await Task.FromResult(_customers.FirstOrDefault(c => c.Cpf == cpf));
        }
    }
}