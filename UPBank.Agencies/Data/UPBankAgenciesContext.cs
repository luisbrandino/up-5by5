using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UPBank.Models;

namespace UPBank.Agencies.Data
{
    public class UPBankAgenciesContext : DbContext
    {
        public UPBankAgenciesContext (DbContextOptions<UPBankAgenciesContext> options)
            : base(options)
        {
        }

        public DbSet<UPBank.Models.Agency> Agency { get; set; } = default!;
    }
}
