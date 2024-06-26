using Microsoft.EntityFrameworkCore;
using UPBank.Accounts.Data;
using UPBank.Accounts.Operations;
using UPBank.Accounts.Operations.Abstract;
using UPBank.Accounts.Specifications;
using UPBank.Accounts.Specifications.Abstract;
using UPBank.DTOs;
using UPBank.Enums;
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

            GetValidation(transaction.Type).Validate(transaction);

            GetOperation(transaction.Type).Execute(transaction);

            _context.Add(transaction);

            await _context.SaveChangesAsync();

            return transaction;
        }

        private ITransactionValidation GetValidation(EType transactionType)
        {
            return transactionType switch
            {   
                EType.Transfer => new TransferTransactionCreationValidation(),
                EType.Withdraw => new DebitTransactionCreationValidation(),
                EType.Payment => new DebitTransactionCreationValidation(),
                _ => new BaseTransactionCreationValidation()
            };
        }

        private ITransactionOperation GetOperation(EType transactionType)
        {
            return transactionType switch
            {
                EType.Transfer => new TransferTransactionOperation(),
                EType.Deposit => new CreditTransactionOperation(),
                EType.Loan => new CreditTransactionOperation(),
                EType.Withdraw => new DebitTransactionOperation(),
                EType.Payment => new DebitTransactionOperation()
            };
        }

        public async Task<IEnumerable<Transaction>> GetActiveLoansFromAgency(string number)
        {
            var transactions = await _context.Transaction
                .Where(
                    transaction => transaction.Type == EType.Loan &&
                    transaction.EffectiveDate <= DateTime.Now
                )
                .ToListAsync();

            var transactionsFromAgency = new List<Transaction>();

            transactions.ForEach(transaction =>
            {
                var account = _accountService.GetRaw(transaction.OriginNumber).Result;

                if (account == null)
                    return;

                transaction.Origin = account;

                if (account.AgencyNumber == number)
                    transactionsFromAgency.Add(transaction);
            });

            return transactionsFromAgency;
        }
    }
}
