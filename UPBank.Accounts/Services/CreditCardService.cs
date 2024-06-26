using UPBank.Accounts.Data;
using UPBank.Models;
using Microsoft.EntityFrameworkCore;
using UPBank.Accounts.Specifications;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace UPBank.Accounts.Services
{
    internal interface ICreditCardNumberGenerator
    {
        public long Generate();
    }

    internal abstract class BaseCreditCardNumber : ICreditCardNumberGenerator
    {
        public abstract long Generate();

        public long GenerateNumberSequence(int length)
        {
            string sequence = "";

            while (sequence.Length < length)
                sequence += new Random().Next(0, 10).ToString();

            return long.Parse(sequence);
        }

    }

    internal class VisaCreditCardNumber : BaseCreditCardNumber
    {
        public override long Generate()
        {
            return long.Parse($"{4}{GenerateNumberSequence(13)}");
        }
    }

    internal class MasterCardCreditCardNumber : BaseCreditCardNumber
    {
        public override long Generate()
        {
            string[] prefixes = { "51", "52", "53", "54", "55" };
            string prefix = prefixes[new Random().Next(prefixes.Length)];
            return long.Parse($"{prefix}{GenerateNumberSequence(16)}");
        }
    }

    public class CreditCardService
    {
        private readonly UPBankAccountsContext _context;

        public CreditCardService(UPBankAccountsContext context)
        {
            _context = context;
        }

        public string GenerateCardVerificationValue() => new Random().Next(1, 999).ToString("000");

        public async Task<CreditCard> Activate(CreditCard creditCard)
        {
            new CreditCardActivationValidation().Validate(creditCard);

            creditCard.Active = true;

            _context.Entry(creditCard).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return creditCard;
        }

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

            _context.CreditCard.Add(creditCard);
            await _context.SaveChangesAsync();

            return creditCard;
        }

        public async Task<CreditCard?> Get(long creditCardNumber) => await _context.CreditCard.FindAsync(creditCardNumber);
    }
}
