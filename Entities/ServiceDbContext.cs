using Microsoft.EntityFrameworkCore;

namespace shipping_service.Entities
{
    public class ServiceDbContext : DbContext
    {

        public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options)
        {

        }

        public DbSet<Courier> Courier { get; set; }
        public DbSet<Package> Package { get; set; }
        public DbSet<PostMachine> PostMachine { get; set; }
        public DbSet<Sender> Sender { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Needs to be mapped. Example: https://github.com/Boulderis/Locals-Trade/blob/main/Support_Your_Locals/Models/ServiceDbContext.cs
            // Also, maybe different voids than ModelCreating could be considered, if necessary.
        }

    }
}