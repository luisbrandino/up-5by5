using UPBank.DTOs;
using UPBank.Models;

namespace UPBank.Agencies.APIs.AddressesAPI.Interface
{
    public interface IAddressService
    {
        public Task<Address?> GetAddressByZipcode(string zipcode);

        public Task<Address?> PostAddressFromDTO(AddressDTO addressDTO);
    }
}