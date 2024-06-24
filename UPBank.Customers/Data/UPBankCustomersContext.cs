using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UPBank.Models;

namespace UPBank.Customers.Data
{
    public class UPBankCustomersContext : DbContext
    {
        public UPBankCustomersContext (DbContextOptions<UPBankCustomersContext> options)
            : base(options)
        {
        }

        public DbSet<UPBank.Models.Customer> Customer { get; set; } = default!;

        public DbSet<UPBank.Models.Address> Addresses { get; set; } = default!;

    }
}
