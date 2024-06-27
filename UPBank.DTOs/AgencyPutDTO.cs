namespace UPBank.DTOs
{
    public class AgencyPutDTO
    {
        public string Number { get; set; }
        public string Cnpj { get; set; }
        public bool Restriction { get; set; }
        public string AddressZipcode { get; set; }

    }
}