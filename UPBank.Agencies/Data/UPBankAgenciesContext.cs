using Microsoft.EntityFrameworkCore;
using UPBank.Models;

namespace UPBank.Agencies.Data
{
    public class UPBankAgenciesContext : DbContext
    {
        public UPBankAgenciesContext(DbContextOptions<UPBankAgenciesContext> options)
            : base(options)
        {
        }

        public DbSet<UPBank.Models.Agency> Agency { get; set; } = default!;
        public DbSet<UPBank.Models.DeletedAgency> DeletedAgency { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agency>()
                        .ToTable("Agency");

            modelBuilder.Entity<DeletedAgency>()
                        .ToTable("DeletedAgency");
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<Agency>())
            {
                if (entry.State == EntityState.Added)
                {
                    var agency = entry.Entity;

                    var deletedAgency = Set<DeletedAgency>().FirstOrDefault(deleted => deleted.Cnpj == agency.Cnpj);

                    if (deletedAgency != null)
                        Set<DeletedAgency>().Remove(deletedAgency);
                }
                
                else if (entry.State == EntityState.Deleted)
                {
                    var deletedAgency = new DeletedAgency
                    {
                        Number = entry.OriginalValues.GetValue<string>("Number"),
                        Cnpj = entry.OriginalValues.GetValue<string>("Cnpj"),
                        Restriction = entry.OriginalValues.GetValue<bool>("Restriction"),
                        Employees = entry.OriginalValues.GetValue<List<Employee>>("Employees"),
                        Address = entry.OriginalValues.GetValue<Address>("Address"),
                        AddressZipcode = entry.OriginalValues.GetValue<string>("AddressZipcode"),
                    };

                    Set<DeletedAgency>().Add(deletedAgency);
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}