namespace UPBank.Addresses.PostalServices.Abstract
{
    public interface IPostalAddressService
    {
        Task<IAddressResult?> Fetch(string zipcode);
    }
}
