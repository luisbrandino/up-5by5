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
        public DbSet<UPBank.Models.Deleted> Deleted { get; set; } = default!;

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<Agency>())
            {
                if (entry.State == EntityState.Added)
                {
                    var agency = entry.Entity;

                    var deletedAgency = Set<Deleted>().FirstOrDefault(deleted => deleted.Cnpj == agency.Cnpj);

                    if (deletedAgency != null)
                        Set<Deleted>().Remove(deletedAgency);
                }

                else if (entry.State == EntityState.Deleted)
                {
                    var deletedAgency = new Deleted
                    {
                        Number = entry.OriginalValues.GetValue<string>("Number"),
                        Cnpj = entry.OriginalValues.GetValue<string>("Cnpj"),
                        Restriction = entry.OriginalValues.GetValue<bool>("Restriction"),
                        AddressZipcode = entry.OriginalValues.GetValue<string>("AddressZipcode"),
                    };

                    Set<Deleted>().Add(deletedAgency);
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}