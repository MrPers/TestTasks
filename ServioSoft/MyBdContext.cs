using Microsoft.EntityFrameworkCore;

namespace ServioSoft
{
    internal class MyDbContext : DbContext
    {
        public DbSet<Guest> GetGuests { get; set; }
        public DbSet<Company> GetCompanies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=KHOTSKYI;Database=TestDb;User Id=sa;Password=9391zif3500; Encrypt=False;Trusted_Connection=True;");
            optionsBuilder.UseSqlServer(@"Server=KHOTSKYI;Database=TestDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guest>()
                .HasOne(sc => sc.Company)
                .WithMany(s => s.Guests)
                .HasForeignKey(sc => sc.CompanyID)
                .HasPrincipalKey(sc => sc.CompanyID)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

    }
}
