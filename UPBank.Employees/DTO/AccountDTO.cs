using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using UPBank.Enums;
using UPBank.Models;

namespace UPBank.Employees.DTO
{
    public class AccountDTO
    {
        public string Number { get; set; }
        public bool Restriction { get; set; }
        public double Overdraft { get; set; }
        public EProfile Profile { get; set; }
        public DateTime CreationDate { get; set; }
        public double Balance { get; set; }

        [JsonIgnore]
        public string AgencyNumber { get; set; }
    }
}
