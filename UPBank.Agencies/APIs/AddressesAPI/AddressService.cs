using UPBank.Agencies.APIs.AddressesAPI.Interface;
using UPBank.Agencies.APIs.Utils;
using UPBank.DTOs;
using UPBank.Models;

namespace UPBank.Agencies.APIs.AddressesAPI
{
    public class AddressService : IAddressService
    {
        private readonly string _Address = "https://localhost:7004/api/Addresses/";

        public async Task<Address> GetAddressByZipcode(string zipcode)
        {
            var url = _Address + "zipcode/" + zipcode;

            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            return await ApiUtils<Address>.GetObjectFromResponse(response) ?? new Address();
        }

        public async Task<Address> PostAddressFromDTO(AddressDTO addressDTO)
        {
            var url = _Address;

            using var client = new HttpClient();
            var response = await client.PostAsJsonAsync(url, addressDTO);

            return await ApiUtils<Address>.GetObjectFromResponse(response) ?? new Address();
        }
    }
}