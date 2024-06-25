using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Text.Json.Serialization;
using UPBank.Models;

namespace UPBank.DTOs
{
    public class AgencyDTO
    {
        public string Cnpj { get; set; }
        public Employee Manager { get; set; }
        public AddressDTO Address { get; set; }
    }
}