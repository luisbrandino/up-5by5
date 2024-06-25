using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPBank.Accounts.Api.Agency.Abstract;
using UPBank.Accounts.Api.Customer.Abstract;
using UPBank.Accounts.Data;
using UPBank.DTOs;
using UPBank.Models;

namespace UPBank.Accounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UPBankAccountsContext _context;
        private readonly ICustomerApi _customer;
        private readonly IAgencyApi _agency;

        public AccountsController(UPBankAccountsContext context, ICustomerApi customer, IAgencyApi agency)
        {
            _context = context;
            _customer = customer;
            _agency = agency;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccount()
        {
            if (_context.Account == null)
            {
                return NotFound();
            }
            IEnumerable<Models.Agency> agencies = await _agency.Get();
            List<Agency> agenciesAll = new List<Agency>();

            var accounts = await _context.Account.ToListAsync();
            foreach (var account in accounts)
            {
               
                Models.Agency agencie = await _agency.Get(account.AgencyNumber);
                //List<Agency> agenciesAll = new List<Agency>();
                var customersinaccount = await _context.AccountCustomer.Where(ac => ac.AccountNumber == account.Number).ToListAsync();
                var customers = new List<Customer>();
                foreach (var customer in customersinaccount)
                {
                    customers.Add(await _customer.Get(customer.CustomerCpf));
                }
                //account.Agency = agencie;
                account.Customers = customers;
                account.Agency = agencie;
            }
            return accounts;        
            
            //foreach (var agency in agencies)
            //{
            //    var agenciesDTO = new Agency();

            //    agenciesDTO.Number = agency.Number;
            //    //agenciesDTO.Address = agency.Address;
            //    //agenciesDTO.AddressZipcode = agency.AddressZipcode;
            //    //agenciesDTO.Cnpj = agency.Cnpj;
            //    //agenciesDTO.Employees = agency.Employees;
            //    //agenciesDTO.Restriction = agency.Restriction;

            //    agenciesAll.Add(agenciesDTO);
            //}
            //return agenciesAll;
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(string? id)
        {
            if (_context.Account == null)
            {
                return NotFound();
            }

            if (id == null)
            {
                return NotFound();
            }

            //Account accountCreated = new Account()
            //{
            //    Number = "101", AgencyNumber = "10001", Balance = 2010, CreationDate = DateTime.Now, Overdraft = 100, Profile = Enums.EProfile.Vip , CreditCardNumber = "101010"
            //};
            //_context.Add(accountCreated);
            //await _context.SaveChangesAsync();

            AccountCustomer accountcostumer = new AccountCustomer()
            {
                CustomerCpf = "000.000.000-03",
                AccountNumber = "101"
            };
            _context.Add(accountcostumer);
            await _context.SaveChangesAsync();


            var account = _context.Account.Find(id);
            Models.Agency agencie = await _agency.Get(account.AgencyNumber);
            //List<Agency> agenciesAll = new List<Agency>();
            var customersinaccount = await _context.AccountCustomer.Where(ac => ac.AccountNumber == account.Number).ToListAsync();
            var customers = new List<Customer>();
            foreach (var customer in customersinaccount)
            {
                customers.Add(await _customer.Get(customer.CustomerCpf));
            }
            account.Agency = agencie;        
            account.Customers = customers;
            return account;
        }

        //}

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        public void ValidateCreationRequest(Account account)
        {
            if (account.Agency == null)
                throw new Exception("Agência não cadastrada");

            if (account.Customers.Any(customer => customer == null))
                throw new Exception("Cliente não cadastrado");

            if (account.Customers.Count > 2)
                throw new Exception("Uma conta pode ter no máximo dois clientes");

            if (account.Customers.Count != account.Customers.Distinct().Count())
                throw new Exception("Não pode haver clientes repetidos");

            Customer firstCustomer = account.Customers.First();

            int age = DateTime.Today.Year - firstCustomer.BirthDate.Year;

            if (firstCustomer.BirthDate > DateTime.Today.AddYears(-age))
                age--;

            if (age <= 18)
                throw new Exception("O primeiro cliente da conta deve ser maior de idade");
        }

        [HttpPost]
        public async Task<ActionResult<Account>> Post(AccountCreationDTO account)
        {
            Agency? agency = await _agency.Get(account.AgencyNumber);

            var tasks = account.Customers.Select(cpf => _customer.Get(cpf));
            List<Customer?> customers = (await Task.WhenAll(tasks)).ToList();

            Account createdAccount = new Account
            {
                Number = "100001",
                Overdraft = 3333,
                Agency = agency,
                AgencyNumber = agency?.Number,
                Customers = customers,
                Profile = account.Profile,
                CreationDate = DateTime.Now,
                Balance = 2000,
                Restriction = true,
                CreditCard = null,
                CreditCardNumber = null
            };

            try
            {
                ValidateCreationRequest(createdAccount);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            account.Customers.ForEach(cpf =>
            {
                AccountCustomer accountCustomer = new AccountCustomer
                {
                    AccountNumber = createdAccount.Number,
                    CustomerCpf = cpf
                };

                // add accountcustomers
            });

            // add account
            
            return createdAccount;
        }

        // DELETE: api/Accounts/5
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
