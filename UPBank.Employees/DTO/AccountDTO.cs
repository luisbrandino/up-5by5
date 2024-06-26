using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using UPBank.Enums;
using UPBank.Models;

namespace UPBank.Employees.DTO
{
    public class AccountDTO
    {
        public bool Restriction { get; set; }

        [JsonIgnore]
        public string AgencyNumber { get; set; }
        public string AccountNumber { get; set; }
        public string EmployeeCPF { get; set; }
    }
}
