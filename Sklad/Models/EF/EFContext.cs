using Microsoft.EntityFrameworkCore;

namespace Sklad.Models.EF
{
    public class EFContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Outcome> Outcomes { get; set; }

        public EFContext(DbContextOptions<EFContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}