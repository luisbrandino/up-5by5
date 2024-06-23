using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using UPBank.Models;

namespace UPBank.Employees.DTO
{
    public class EmployeeDTO : Person
    {
        public int Register;
        public bool Manager { get; set; }

        [NotMapped]
        public Agency Agency { get; set; }

        [JsonIgnore]
        public string AgencyNumber { get; set; }
    }
}
