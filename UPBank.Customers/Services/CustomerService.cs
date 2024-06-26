using Microsoft.AspNetCore.Mvc;
using UPBank.Customers.Repositories;
using UPBank.DTOs;
using UPBank.Models;
using UPBank.Models.Utils;

namespace UPBank.Customers.Services
{
    public class CustomerService
    {
        private CustomerRepository _customerRepository;

        public CustomerService()
        {
            _customerRepository = new();
        }

        public async Task<List<Customer>> GetAll()
        {
            return await _customerRepository.GetAll();
        }

        public async Task<Customer> GetByCpf(string cpf)
        {
            return await _customerRepository.GetByCPF(cpf);
        }

        public async Task<Customer> PostCustomer(CustomersDTO dto)
        {
            Customer customer = new Customer(dto);
            customer.Address = returnAddress(dto.Address);

            if (RestoreCustomer(customer))
            {
                await _customerRepository.RestoreCustomer(customer);
                return customer;
            }

            if (IsCustomerUnique(customer))
            {
                await _customerRepository.PostCustomer(customer);
                return customer;
            }

            throw new Exception("Usuario já está cadastrado");
        }

        public async Task<bool> EditCustomer(CustomersDTO dto)
        {
            Customer updatedCustomer = new Customer(dto);
            updatedCustomer.Address = returnAddress(dto.Address);

           
            return await _customerRepository.EditCustomer(updatedCustomer);
        }

        public async Task<Customer> DeleteCustomer(string cpf)
        {
            return await _customerRepository.DeleteCustomer(cpf);
        }

        public async Task<bool> ChangeRestriction(string cpf)
        {
            return await _customerRepository.ChangeRestriction(cpf);
        }
        private Address returnAddress(string zipcode)
        {
            try
            {
                string baseUri = "https://localhost:7004";
                string requestUri = $"/api/addresses/zipcode/{zipcode}";

                var addressReturn = ApiConsume<Address>.Get(baseUri, requestUri).Result;

                if (addressReturn == null)
                {
                    requestUri = $"/api/addresses";

                    AddressDTO dto = new AddressDTO()
                    {
                        Zipcode = zipcode,
                        Number = 1,
                        Complement = null,
                    };
                    addressReturn = ApiConsume<Address>.Post(baseUri, requestUri, dto).Result;
                }

                addressReturn.Zipcode = zipcode;

                return addressReturn;

            } catch (Exception ex)
            {
                throw new Exception("Endereço invalido");
            }
          
        }

        private bool IsCustomerUnique(Customer customer)
        {
            HashSet<string> cpfSet = new HashSet<string>();

            var customers = _customerRepository.GetAll().Result;
            foreach (var cust in customers)
            {
                cpfSet.Add(cust.Cpf);
            }

            return !cpfSet.Contains(customer.Cpf);
        }

        private bool RestoreCustomer(Customer customer)
        {
            HashSet<string> cpfSet = new HashSet<string>();

            var deletedCustomers = _customerRepository.GetAllDeleted().Result;
            foreach (var deletedCust in deletedCustomers)
            {
                cpfSet.Add(deletedCust.Cpf);
            }

            return cpfSet.Contains(customer.Cpf);
        }
    }
}

