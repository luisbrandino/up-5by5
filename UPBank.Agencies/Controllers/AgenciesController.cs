using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPBank.Agencies.Services;
using UPBank.Agencies.Data;
using UPBank.Enums;
using UPBank.Models;
using UPBank.Agencies.APIs.EmployeesAPI.Interface;
using UPBank.Agencies.APIs.AddressesAPI.Interface;
using UPBank.Agencies.APIs.AccountsAPI.Interface;
using UPBank.Agencies.Validations;
using UPBank.DTOs;

namespace UPBank.Agencies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgenciesController : ControllerBase
    {
        private readonly UPBankAgenciesContext _context;
        private readonly AgencyService _agencyService;

        public AgenciesController(UPBankAgenciesContext context, AgencyService service)
        {
            _context = context;
            _agencyService = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agency>>> GetAgency()
        {
            if (_context.Agency == null)
                return NotFound();

            List<Agency> agencies = await _context.Agency.ToListAsync();

            foreach (var agency in agencies)
                _ = Task.Run(() => _agencyService.FillData(agency));

            await Task.WhenAll(agencies.Select(agency => _agencyService.FillData(agency)));

            return agencies;
        }

        [HttpGet("agency/{agencyNumber}")]
        public async Task<ActionResult<Agency>> GetAgency(string agencyNumber)
        {
            if (_context.Agency == null)
                return NotFound();

            var agency = await _context.Agency.FirstOrDefaultAsync(a => a.Number == agencyNumber);

            if (agency == null)
                return NotFound();

            await _agencyService.FillData(agency);

            return agency;
        }

        [HttpGet("{agencyNumber}/Restricteds")]
        public async Task<ActionResult<IEnumerable<Account>>> GetRestrictedAccounts(string agencyNumber)
        {
            try
            {
                var accounts = await _agencyService.GetRestrictedAccounts(agencyNumber);

                if (!accounts.Any())
                    return Problem("There are no accounts restricted!");

                return accounts.ToList();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{agencyNumber}/ByProfile/{profile}")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccountsByProfile(string agencyNumber, EProfile profile)
        {
            try
            {
                var accounts = await _agencyService.GetAccountsByProfile(agencyNumber, profile);

                if (!accounts.Any())
                    return Problem("There are no accounts that match this profile!");

                return accounts.ToList();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{agencyNumber}/WithActiveOverdraft")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccountsWithActiveOverdraft(string agencyNumber)
        {
            try
            {
                var accounts = await _agencyService.GetAccountsWithActiveOverdraft(agencyNumber);

                if (!accounts.Any())
                    return Problem("There are no accounts with active overdraft!");

                return accounts.ToList();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("{agencyNumber}")]
        public async Task<IActionResult> PutAgency(string agencyNumber, Agency agency)
        {
            if (agencyNumber != agency.Number)
                return BadRequest();

            try
            {
                AgencyValidator.Validate(agency);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            _context.Entry(agency).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgencyExists(agencyNumber))
                    return NotFound();

                else
                    throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Agency>> PostAgency(AgencyDTO dto)
        {
            var agency = await _agencyService.CreateAgencyFromDTO(dto);

            try
            {
                AgencyValidator.Validate(agency);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

            _context.Agency.Add(agency);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AgencyExists(agency.Number))
                    return Conflict();

                else
                    throw;
            }

            return CreatedAtAction("GetAgency", new { agencyNumber = agency.Number }, agency);
        }

        [HttpDelete("{agencyNumber}")]
        public async Task<IActionResult> DeleteAgency(string agencyNumber)
        {
            if (_context.Agency == null)
                return NotFound();

            var agency = await _context.Agency.FindAsync(agencyNumber);

            if (agency == null)
                return NotFound();

            _context.Agency.Remove(agency);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{agencyNumber}")]
        public async Task<IActionResult> PatchAgencyRestriction(string agencyNumber)
        {
            if (_context.Agency == null)
                return NotFound();

            var agency = await _context.Agency.FindAsync(agencyNumber);

            if (agency == null)
                return NotFound();

            agency.Restriction = !agency.Restriction;
            _context.Entry(agency).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgencyExists(agencyNumber))
                    return NotFound();

                else
                    throw;
            }

            return NoContent();
        }

        private bool AgencyExists(string agencyNumber) => (_context.Agency?.Any(e => e.Number == agencyNumber)).GetValueOrDefault();
    }
}