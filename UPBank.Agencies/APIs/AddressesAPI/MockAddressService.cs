using UPBank.Agencies.APIs.AddressesAPI.Interface;
using UPBank.DTOs;
using UPBank.Models;

namespace UPBank.Agencies.APIs.AddressesAPI
{
    public class MockAddressService : IAddressService
    {
        private readonly List<Address> _addresses;

        public MockAddressService()
        {
            _addresses = new(new List<Address>
            {
                new() {
                    Number = 1,
                    Street = "Rua 1",
                    Complement = "Casa 1",
                    Neighborhood = "Bairro 1",
                    City = "Cidade 1",
                    State = "Estado 1",
                    Zipcode = "11111-111"
                },
                new() {
                    Number = 2,
                    Street = "Rua 2",
                    Complement = "Casa 2",
                    Neighborhood = "Bairro 2",
                    City = "Cidade 2",
                    State = "Estado 2",
                    Zipcode = "22222-222"
                },
                new() {
                    Number = 3,
                    Street = "Rua 3",
                    Complement = "Casa 3",
                    Neighborhood = "Bairro 3",
                    City = "Cidade 3",
                    State = "Estado 3",
                    Zipcode = "33333-333"
                }
            });
        }

        public async Task<Address> GetAddressByZipcode(string zipcode) => Task.FromResult(_addresses.FirstOrDefault(address => address.Zipcode == zipcode)).Result ?? new Address();

        public async Task<Address> PostAddressFromDTO(AddressDTO addressDTO)
        {
            var address = new Address
            {
                Number = addressDTO.Number,
                Street = "Rua " + addressDTO.Number,
                Complement = addressDTO.Complement,
                Neighborhood = "Bairro " + addressDTO.Number,
                City = "City " + addressDTO.Number,
                State = "State " + addressDTO.Number,
                Zipcode = addressDTO.Zipcode
            };

            if (!_addresses.Contains(address))
            {
                if (address.Zipcode != null)
                    _addresses.Add(address);
            }

            return Task.FromResult(_addresses.FirstOrDefault(a => a.Zipcode == address.Zipcode)).Result ?? new Address();
        }
    }
}