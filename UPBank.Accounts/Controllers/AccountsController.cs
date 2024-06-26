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

        [HttpGet("agency/{number}/restricted")]
        public async Task<ActionResult<IEnumerable<Account>>> GetRestrictedAccounts(string number)
        {
            try
            {
                return (await _service.GetRestrictedAccountsFromAgency(number)).ToList();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("profile/{profile}")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccountsByProfile(EProfile profile)
        {
            try
            {
                return (await _service.GetAccountsByProfile(profile)).ToList();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("activeloans")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccountsWithActiveLoan()
        {
            try
            {
                return (await _service.GetAccountsWithActiveLoan()).ToList();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{number}/transactions/{type}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByType(string number, int type)
        {
            var transactionType = (EType)type;
            try
            {
                var transactions = await _service.GetTransactionsByType(number, transactionType);

                if (transactions == null)
                    return NotFound();

                return transactions.ToList();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{number}/statement")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetAccountStatement(string number)
        {
            try
            {
                var transactions = await _service.GetStatement(number);

                if (transactions == null)
                    return NotFound();

                return transactions.ToList();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{number}/balance")]
        public async Task<ActionResult<Dictionary<string, double>>> GetBalance(string number)
        {
            try
            {
                var account = await _service.GetRaw(number);

                if (account == null)
                    return NotFound();

                return new Dictionary<string, double> { { "Balance", account.Balance } };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{number}")]
        public async Task<ActionResult<Account>> PutAccount(string number, UpdateAccountDTO account)
        {
            try
            {
                var updatedAccount = await _service.Update(number, account);

                if (updatedAccount == null)
                    return NotFound();

                return updatedAccount;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{number}/profile")]
        public async Task<ActionResult<Account>> ChangeProfile(string number, ChangeProfileDTO profile)
        {
            try
            {
                var account = await _service.ChangeProfile(number, profile.Profile);

                if (account == null)
                    return NotFound();

                return account;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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

        [HttpPost("{number}/activate")]
        public async Task<ActionResult<Account>> Activate(string number)
        {
            try
            {
                var account = await _service.Activate(number);

                if (account == null)
                    return NotFound();

                return account;
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
        [ServiceFilter(typeof(AddNumberOriginToTransactionFilter), Order = int.MinValue)]
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

        [HttpDelete("{number}")]
        public async Task<ActionResult<DeletedAccount>> DeleteAccount(string number)
        {
            try
            {
                var deletedAccount = await _service.Delete(number);

                if (deletedAccount == null)
                    return NotFound();

                return deletedAccount;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private bool AccountExists(string id)
        {
            return (_context.Account?.Any(e => e.Number == id)).GetValueOrDefault();
        }
    }
}
