using UPBank.Enums;

namespace UPBank.DTOs
{
    public class AccountCreationDTO
    {
        public double Overdraft { get; set; }
        public EProfile Profile { get; set; }
        public string AgencyNumber { get; set; }
        public bool IsSavingsAccount { get; set; } = false;
        public List<string> Customers { get; set; } // apenas CPF
    }
}
