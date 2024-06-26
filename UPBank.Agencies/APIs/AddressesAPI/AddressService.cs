using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using UPBank.Agencies.APIs.AddressesAPI.Interface;
using UPBank.DTOs;
using UPBank.Models;

namespace UPBank.Agencies.APIs.AddressesAPI
{
    public class AddressService : IAddressService
    {
        private readonly string _Address = "https://localhost:7004/api/Addresses/";

        public async Task<Address?> GetAddressByZipcode(string zipcode)
        {
            var url = _Address + "zipcode/" + zipcode;

            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            return await Deserialize(response);
        }

        public async Task<Address?> PostAddressFromDTO(AddressDTO addressDTO)
        {
            var url = _Address;

            using var client = new HttpClient();
            var response = await client.PostAsJsonAsync(url, addressDTO);

            return await Deserialize(response);
        }

        private async Task<Address?> Deserialize(HttpResponseMessage response)
        {
            Address? address = null;

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(json))
                    address = JsonConvert.DeserializeObject<Address>(json);
            }

            return address;
        }
    }
}