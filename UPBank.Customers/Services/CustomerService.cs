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

        public async Task<CustomersDTO> Post(CustomersDTO dto)
        {
            Customer customer = new Customer(dto);
            customer.Address = returnAddress(dto.Address);


            _customerRepository.PostCustomer(customer);

            return dto;

        }

        private Address returnAddress(string zipcode)
        {
            string baseUri = "https://localhost:7004";
            string requestUri = $"/api/addresses/zipcode/{zipcode}";

            var addressReturn = ApiConsume<Address>.Get(baseUri, requestUri).Result;
            addressReturn.Zipcode = zipcode;
            return addressReturn;
        }

        /*      private bool ValideCustomer(Customer customer)
              {
                  HashSet<string> cpfLists = new HashSet<string>();

                  var ListSale = _saleRepository.GetSale().Result;


                  var filteredSales = ListSale.Where(s => s.Flight.Id == sale.Flight.Id).ToList();


                  foreach (var passenger in sale.Passengers.Concat(filteredSales.SelectMany(s => s.Passengers)))
                  {
                      if (!cpfLists.Add(passenger.CPF))
                      {
                          return false;
                      }
                  }

                  return true;
              }*/


    }
}

