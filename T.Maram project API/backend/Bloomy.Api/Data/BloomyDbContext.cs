using Microsoft.EntityFrameworkCore;

namespace Bloomy.Api.Data
{
    public partial class BloomyDbContext : DbContext
    {
        public BloomyDbContext(DbContextOptions<BloomyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ConfigureCatalog(modelBuilder);
            ConfigureSales(modelBuilder);
        }

        partial void ConfigureCatalog(ModelBuilder modelBuilder);
        partial void ConfigureSales(ModelBuilder modelBuilder);
    }
}