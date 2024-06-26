using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Text.Json.Serialization;

namespace UPBank.DTOs
{
    public class AgencyDTO
    {
        public string Number { get; set; }
        public string Cnpj { get; set; }
        public AgencyCreationEmployeeDTO Manager { get; set; }
        public AddressDTO Address { get; set; }
    }
}