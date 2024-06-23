using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UPBank.Models;

namespace UPBank.Employees.Data
{
    public class UPBankEmployeesContext : DbContext
    {
        public UPBankEmployeesContext (DbContextOptions<UPBankEmployeesContext> options)
            : base(options)
        {
        }

        public DbSet<UPBank.Models.Employee> Employee { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .ToTable("People");

            modelBuilder.Entity<Employee>()
                .HasBaseType<Person>(); 
        }
    }
}
