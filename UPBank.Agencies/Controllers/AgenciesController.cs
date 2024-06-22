using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPBank.Agencies.Data;
using UPBank.Models;

namespace UPBank.Agencies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgenciesController : ControllerBase
    {
        private readonly UPBankAgenciesContext _context;

        public AgenciesController(UPBankAgenciesContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agency>>> GetAgency()
        {
          if (_context.Agency == null)
          {
              return NotFound();
          }
            return await _context.Agency.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Agency>> GetAgency(string id)
        {
          if (_context.Agency == null)
          {
              return NotFound();
          }
            var agency = await _context.Agency.FindAsync(id);

            if (agency == null)
            {
                return NotFound();
            }

            return agency;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgency(string id, Agency agency)
        {
            if (id != agency.Number)
            {
                return BadRequest();
            }

            _context.Entry(agency).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgencyExists(id))
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
        public async Task<ActionResult<Agency>> PostAgency(Agency agency)
        {
          if (_context.Agency == null)
          {
              return Problem("Entity set 'UPBankAgenciesContext.Agency'  is null.");
          }
            _context.Agency.Add(agency);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AgencyExists(agency.Number))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAgency", new { id = agency.Number }, agency);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgency(string id)
        {
            if (_context.Agency == null)
            {
                return NotFound();
            }
            var agency = await _context.Agency.FindAsync(id);
            if (agency == null)
            {
                return NotFound();
            }

            _context.Agency.Remove(agency);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AgencyExists(string id)
        {
            return (_context.Agency?.Any(e => e.Number == id)).GetValueOrDefault();
        }
    }
}
