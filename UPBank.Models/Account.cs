using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using UPBank.Enums;

namespace UPBank.Models
{
    public class Account
    {
        [Key]
        public string Number { get; set; }
        public bool Restriction {  get; set; }
        public double Overdraft { get; set; }
        public EProfile Profile { get; set; }
        public DateTime CreationDate { get; set; }
        public double Balance { get; set; }
        public string? SavingsAccount { get; set; } // Conta poupança, opcional

        [NotMapped]
        public CreditCard CreditCard { get; set; }

        [JsonIgnore]
        public string CreditCardNumber { get; set; }

        [NotMapped]
        public Agency Agency { get; set; }

        [JsonIgnore]
        public string AgencyNumber { get; set; }

        [NotMapped]
        public List<Transaction> Transactions { get; set; }

        [NotMapped]
        public List<Customer> Customers { get; set; }
    }
}
