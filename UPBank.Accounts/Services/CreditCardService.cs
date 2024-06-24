using UPBank.Accounts.Data;
using UPBank.Models;

namespace UPBank.Accounts.Services
{
    internal interface ICreditCardNumberGenerator
    {
        public long Generate();
    }

    internal class VisaCreditCardNumber : ICreditCardNumberGenerator
    {
        public long Generate()
        {
            return 4;
        }
    }

    internal class MasterCardCreditCardNumber : ICreditCardNumberGenerator
    {
        public long Generate()
        {
            return 5;
        }
    }

    public class CreditCardService
    {
        private readonly UPBankAccountsContext _context;

        public CreditCardService(UPBankAccountsContext context)
        {
            _context = context;
        }

        public string GenerateCreditCardNumber()
        {
            return string.Empty;
        }

        public string GenerateCardVerificationValue() => new Random().Next(1, 999).ToString("000");

        public async Task<CreditCard> Create(string holder)
        {
            CreditCard creditCard = new CreditCard()
            {
                CVV = GenerateCardVerificationValue(),
                Limit = 500,  
                ExtractionDate = DateTime.Now.AddYears(10),
                Holder = holder,
                Active = false,
            };

            var creditCardNumberGenerators = new Dictionary<string, ICreditCardNumberGenerator>()
            {
                { "Visa", new VisaCreditCardNumber() },
                { "MasterCard", new MasterCardCreditCardNumber() }
            };

            var creditCardNumberGenerator = creditCardNumberGenerators.OrderBy(_ => Guid.NewGuid()).First();

            creditCard.Brand = creditCardNumberGenerator.Key;
            creditCard.Number = creditCardNumberGenerator.Value.Generate();

            return await Task.FromResult(creditCard);
        }

    }
}
