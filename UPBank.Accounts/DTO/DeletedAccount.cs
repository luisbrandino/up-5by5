using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using UPBank.Enums;
using UPBank.Models;

namespace UPBank.Accounts.DTO
{
    public class DeletedAccount
    {
        [Key]
        public string Number { get; set; }
        public bool Restriction { get; set; }
        public double Overdraft { get; set; }
        public EProfile Profile { get; set; }
        public DateTime CreationDate { get; set; }
        public double Balance { get; set; }
        public string? SavingsAccount { get; set; } 

        [JsonIgnore]
        public long CreditCardNumber { get; set; }

        [JsonIgnore]
        public string AgencyNumber { get; set; }
    }
}
