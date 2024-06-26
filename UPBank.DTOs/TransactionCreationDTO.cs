using UPBank.Enums;

namespace UPBank.DTOs
{
    public class TransactionCreationDTO
    {
        public EType Type { get; set; }
        public double Value { get; set; }
        public string OriginNumber { get; set; }
        public string? DestinyNumber { get; set; }
    }
}
