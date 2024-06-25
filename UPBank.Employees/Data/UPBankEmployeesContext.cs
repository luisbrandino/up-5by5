using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UPBank.Employees.DTO;
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
        public DbSet<DeletedEmployee> DeletedEmployee { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>().Ignore(e => e.Address);
            modelBuilder.Entity<Employee>().Ignore(e => e.Agency);

            modelBuilder.Entity<DeletedEmployee>(entity =>
            {
                entity.ToTable("DeletedEmployee");
            });
        }
    }
}

/*
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");
                entity.Ignore(e => e.Address);
                entity.Ignore(e => e.Agency);
            });

            modelBuilder.Entity<DeletedEmployee>(entity =>
            {
                entity.ToTable("DeletedEmployee");
                entity.Ignore(e => e.Address);
                entity.Ignore(e => e.Agency);
            });
 */