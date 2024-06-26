using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UPBank.Models
{
    public class Deleted 
    {
        [Key]
        public string Number { get; set; }
        public string Cnpj { get; set; }
        public bool Restriction { get; set; }

        [NotMapped]
        public List<Employee> Employees { get; set; }

        [NotMapped]
        public Address Address { get; set; }

        [JsonIgnore]
        public string AddressZipcode { get; set; }
    }
}