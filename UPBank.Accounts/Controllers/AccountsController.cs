using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPBank.Accounts.Api.Agency.Abstract;
using UPBank.Accounts.Data;
using UPBank.Accounts.Filters;
using UPBank.Accounts.Services;
using UPBank.DTOs;
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

        /*
        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccount()
        {
          if (_context.Account == null)
          {
              return NotFound();
          }
            List<Account> accounts = await _context.Account.ToListAsync();

            foreach (var account in accounts)
            {
                string number = account.AgencyNumber;
                account.Agency = await _agency.Get(number);
            }

            return accounts;
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(string id)
        {
          if (_context.Account == null)
          {
              return NotFound();
          }
            var account = await _context.Account.FindAsync(id);

            account.Agency = await _agency.Get(account.AgencyNumber);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }
        */
       
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            if (_context.Account == null)
            {
                return NotFound();
            }
            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Account.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(string id)
        {
            return (_context.Account?.Any(e => e.Number == id)).GetValueOrDefault();
        }
    }
}
