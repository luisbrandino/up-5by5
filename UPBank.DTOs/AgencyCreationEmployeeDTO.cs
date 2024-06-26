namespace UPBank.DTOs
{
    public class AgencyCreationEmployeeDTO : PersonsDTO
    {
        public int Register;
        public string AgencyNumber { get; set; }
        public bool Manager { get; set; } = true;
        public string AddressZipcode { get; set; }
    }
}
