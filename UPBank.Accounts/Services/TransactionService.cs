using UPBank.Accounts.Data;
using UPBank.Accounts.Specifications;
using UPBank.DTOs;
using UPBank.Models;

namespace UPBank.Accounts.Services
{
    public class TransactionService
    {
        private readonly UPBankAccountsContext _context;
        private readonly AccountService _accountService;

        public TransactionService(UPBankAccountsContext context, AccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        public async Task<Transaction> MakeFromRequestedTransaction(TransactionCreationDTO requestedTransaction)
        {
            var transaction = new Transaction()
            {
                Type = requestedTransaction.Type,
                Value = requestedTransaction.Value,
                EffectiveDate = DateTime.Now
            };

            transaction.Origin = await _accountService.GetRaw(requestedTransaction.OriginNumber);

            if (transaction.Origin != null)
                transaction.OriginNumber = transaction.Origin.Number;

            if (requestedTransaction.DestinyNumber != null)
                transaction.Destiny = await _accountService.GetRaw(requestedTransaction.DestinyNumber);

            transaction.DestinyNumber = requestedTransaction.DestinyNumber;

            return transaction;
        }

        public async Task<Transaction> Create(TransactionCreationDTO requestedTransaction)
        {
            Transaction transaction = await MakeFromRequestedTransaction(requestedTransaction);

            new TransactionCreationValidation().Validate(transaction);

            _context.Transaction.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

    }
}
