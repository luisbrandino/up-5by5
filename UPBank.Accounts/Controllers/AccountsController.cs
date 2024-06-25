using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPBank.Accounts.Api.Agency.Abstract;
using UPBank.Accounts.Api.Customer.Abstract;
using UPBank.Accounts.Data;
using UPBank.Accounts.DTO;
using UPBank.Accounts.Filters;
using UPBank.Accounts.Services;
using UPBank.DTOs;
using UPBank.Enums;
using UPBank.Models;

namespace UPBank.Accounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UPBankAccountsContext _context;
        private readonly TransactionService _transactionService;
        private readonly AccountService _service;

        public AccountsController(UPBankAccountsContext context, AccountService service, TransactionService transactionService)
        {
            _context = context;
            _service = service;
            _transactionService = transactionService;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccount()
        {
            try
            {
                return (await _service.Get()).ToList();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{number}")]
        public async Task<ActionResult<Account?>> GetAccount(string number)
        {
            try
            {
                var account = await _service.Get(number);

                if (account == null)
                    return NotFound();

                return account;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(string id, Account account)
        {
            if (id != account.Number)
            {
                return BadRequest();
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Account>> Post(AccountCreationDTO requestedAccount)
        {
            try
            {
                return await _service.Create(requestedAccount);
            } 
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("{number}/creditcard/activate")]
        public async Task<ActionResult<CreditCard>> ActivateCreditCard(string number)
        {
            try
            {
                return await _service.ActivateCreditCard(number);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("{number}/transactions")]
        [ServiceFilter(typeof(PopulateOriginNumberActionFilter))]
        public async Task<ActionResult<Transaction>> MakeTransaction(TransactionCreationDTO requestedTransaction)
        {
            try
            {
                return await _transactionService.Create(requestedTransaction);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{Number}")]
        public async Task<IActionResult> DeleteAccount(string Number)
        {
            if (_context.Account == null)
            {
                return NotFound("Account not found");
            }
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var account = await _context.Account.FindAsync(Number);

                if (account == null)
                {
                    return NotFound();
                }

                var deletedAccount = CreateDeletedAccountFromAccount(account);
                _context.DeletedAccount.Add(deletedAccount);
                _context.Account.Remove(account);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return NoContent();

            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Internal server error");
            }
        }

        private bool AccountExists(string id)
        {
            return (_context.Account?.Any(e => e.Number == id)).GetValueOrDefault();
        }

        public static DeletedAccount CreateDeletedAccountFromAccount (Account account)
        {
            return new DeletedAccount
            {
                Number = account.Number,
                Restriction = account.Restriction,
                Overdraft = account.Overdraft,
                Profile = account.Profile,
                CreationDate = account.CreationDate,
                Balance = account.Balance,
                SavingsAccount = account.SavingsAccount,
                CreditCardNumber = account.CreditCardNumber,
                AgencyNumber = account.AgencyNumber
            };
        }
    }
}
