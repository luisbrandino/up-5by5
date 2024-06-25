using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UPBank.Accounts.DTO;
using UPBank.Models;

namespace UPBank.Accounts.Data
{
    public class UPBankAccountsContext : DbContext
    {
        public UPBankAccountsContext (DbContextOptions<UPBankAccountsContext> options)
            : base(options)
        {
        }

        public DbSet<DeletedAccount> DeletedAccount { get; set; } = default!;
        public DbSet<CreditCard> CreditCard { get; set; } = default!;
        public DbSet<Account> Account { get; set; } = default!;
        public DbSet<AccountCustomer> AccountCustomer { get; set; } = default!;
        public DbSet<Transaction> Transaction { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AccountCustomer>()
                .HasKey(ac => new { ac.AccountNumber, ac.CustomerCpf });
        }
    }
}
