using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using UPBank.Enums;

namespace UPBank.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime EffectiveDate { get; set; }
        public EType Type { get; set; }
        public double Value;

        [NotMapped]
        public Account? Origin { get; set; }

        [JsonIgnore]
        public string? OriginNumber { get; set; }

        [NotMapped]
        public Account? Destiny { get; set; }

        [JsonIgnore]
        public string? DestinyNumber {  get; set; }
    }
}
