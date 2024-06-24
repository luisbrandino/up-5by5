using System.ComponentModel.DataAnnotations;

namespace UPBank.Models
{
    public class CreditCard
    {
        [Key]
        public long Number { get; set; }
        public DateTime ExtractionDate { get; set; }
        public double Limit { get; set; }
        public string CVV { get; set; }
        public string Holder { get; set; }
        public string Brand { get; set; }
    }
}
