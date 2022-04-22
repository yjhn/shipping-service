using Microsoft.EntityFrameworkCore;

using shipping_service.Persistence.Entities;

namespace shipping_service.Persistence.DatabaseContext
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Courier> Couriers { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PostMachine> PostMachines { get; set; }
        public DbSet<Sender> Senders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // generate created time
            modelBuilder.Entity<Courier>()
                .Property(c => c.Created)
                .HasDefaultValueSql("now()");
            modelBuilder.Entity<Package>()
                .Property(p => p.Created)
                .HasDefaultValueSql("now()");
            modelBuilder.Entity<PostMachine>()
                .Property(p => p.Created)
                .HasDefaultValueSql("now()");
            modelBuilder.Entity<Sender>()
                .Property(s => s.Created)
                .HasDefaultValueSql("now()");

            // generate modified time
            modelBuilder.Entity<Courier>()
                .Property(c => c.Modified)
                .HasComputedColumnSql("now()");
            modelBuilder.Entity<Package>()
                .Property(p => p.Modified)
                .HasComputedColumnSql("now()");
            modelBuilder.Entity<PostMachine>()
                .Property(p => p.Modified)
                .HasComputedColumnSql("now()");
            modelBuilder.Entity<Sender>()
                .Property(s => s.Modified)
                .HasComputedColumnSql("now()");

            // unique indexes
            modelBuilder.Entity<Courier>()
                .HasIndex(c => c.Username)
                .IsUnique();
            modelBuilder.Entity<Sender>()
                .HasIndex(s => s.Username)
                .IsUnique();
            modelBuilder.Entity<PostMachine>()
                .HasIndex(p => p.Name)
                .IsUnique();

            // relationships

            // this property is not required because the user can
            // delete their account while packages are still in transfer
            modelBuilder.Entity<Package>()
                .HasOne(p => p.Sender)
                .WithMany(s => s.Packages)
                .HasForeignKey(p => p.SenderId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Package>()
                .HasOne(p => p.Courier)
                .WithMany(c => c.CurrentPackages)
                .HasForeignKey(p => p.CourierId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Package>()
                .HasOne(p => p.SourceMachine)
                .WithMany(p => p.PackagesWithThisSource)
                .HasForeignKey(p => p.SourceMachineId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Package>()
                .HasOne(p => p.DestinationMachine)
                .WithMany(p => p.PackagesWithThisDestination)
                .HasForeignKey(p => p.DestinationMachineId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // keys
            modelBuilder.Entity<Courier>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Package>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<PostMachine>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Sender>()
                .HasKey(s => s.Id);

            // property types
            modelBuilder.Entity<Courier>()
                .Property(c => c.Username)
                .HasColumnType("varchar(50)")
                .IsRequired();
            modelBuilder.Entity<Courier>()
                .Property(c => c.Name)
                .HasColumnType("varchar(100)")
                .IsRequired();
            modelBuilder.Entity<Courier>()
                .Property(c => c.HashedPassword)
                .HasColumnType("bytea")
                .IsRequired();

            modelBuilder.Entity<PostMachine>()
                .Property(p => p.Name)
                .HasColumnType("varchar(50)")
                .IsRequired();
            modelBuilder.Entity<PostMachine>()
                .Property(p => p.Address)
                .HasColumnType("varchar(100)")
                .IsRequired();

            modelBuilder.Entity<Sender>()
                .Property(s => s.Username)
                .HasColumnType("varchar(50)")
                .IsRequired();
            modelBuilder.Entity<Sender>()
                .Property(s => s.HashedPassword)
                .HasColumnType("bytea")
                .IsRequired();

            modelBuilder.Entity<Package>()
                .Property(p => p.Title)
                .HasColumnType("varchar(50)")
                .IsRequired();
            modelBuilder.Entity<Package>()
                .Property(p => p.Description)
                .HasColumnType("varchar(100)")
                .IsRequired(false);
            modelBuilder.Entity<Package>()
                .Property(p => p.Status)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}