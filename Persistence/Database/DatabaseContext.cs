using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using shipping_service.Persistence.Entities;

namespace shipping_service.Persistence.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Courier> Couriers { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<PostMachine> PostMachines { get; set; }
        public DbSet<Sender> Senders { get; set; }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default
        )
        {
            OnBeforeSaving();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void OnBeforeSaving()
        {
            IEnumerable<EntityEntry> entries = ChangeTracker.Entries();
            DateTime utcNow = DateTime.UtcNow;

            foreach (EntityEntry entry in entries)
            {
                // set modified time on create or update (cannot call "now()" on update
                // as it is not an immutable function, so do this on save manually)
                // see https://www.npgsql.org/efcore/modeling/generated-properties.html#timestamp-generation
                // and https://threewill.com/how-to-auto-generate-created-updated-field-in-ef-core/
                if (entry.Entity is IBaseEntity trackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            // set the updated date to "now"
                            trackable.Modified = utcNow;

                            // mark property as "don't touch"
                            // we don't want to update on a Modify operation
                            // entry.Property("CreatedOn").IsModified = false;
                            break;

                        case EntityState.Added:
                            // set updated date to "now" (created will be set automatically)
                            trackable.Modified = utcNow;
                            break;
                    }
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // keys
            modelBuilder.Entity<Courier>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Shipment>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<PostMachine>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Sender>()
                .HasKey(s => s.Id);

            // generate keys automatically
            modelBuilder.Entity<Courier>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityAlwaysColumn();
            modelBuilder.Entity<Shipment>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityAlwaysColumn();
            modelBuilder.Entity<PostMachine>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityAlwaysColumn();
            modelBuilder.Entity<Sender>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityAlwaysColumn();

            // use optimistic locking (concurrency tokens)
            // https://www.npgsql.org/efcore/modeling/concurrency.html
            // when the same entity is concurrently updated, `DbUpdateConcurrencyException`
            // is thrown on dbContext.SaveChanges[Async]
            // https://microsoft.github.io/PartsUnlimited/arch/200.9x-Arch-OptimisticConcurrency.html
            modelBuilder.Entity<Courier>().UseXminAsConcurrencyToken();
            modelBuilder.Entity<Shipment>().UseXminAsConcurrencyToken();
            modelBuilder.Entity<PostMachine>().UseXminAsConcurrencyToken();
            modelBuilder.Entity<Sender>().UseXminAsConcurrencyToken();

            // generate created time
            modelBuilder.Entity<Courier>()
                .Property(c => c.Created)
                .HasDefaultValueSql("now()");
            modelBuilder.Entity<Shipment>()
                .Property(p => p.Created)
                .HasDefaultValueSql("now()");
            modelBuilder.Entity<PostMachine>()
                .Property(p => p.Created)
                .HasDefaultValueSql("now()");
            modelBuilder.Entity<Sender>()
                .Property(s => s.Created)
                .HasDefaultValueSql("now()");

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
            modelBuilder.Entity<Shipment>()
                .HasOne(p => p.Sender)
                .WithMany(s => s.Shipments)
                .HasForeignKey(p => p.SenderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Shipment>()
                .HasOne(p => p.Courier)
                .WithMany(c => c.CurrentShipments)
                .HasForeignKey(p => p.CourierId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Shipment>()
                .HasOne(p => p.SourceMachine)
                .WithMany(p => p.ShipmentsWithThisSource)
                .HasForeignKey(p => p.SourceMachineId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Shipment>()
                .HasOne(p => p.DestinationMachine)
                .WithMany(p => p.ShipmentsWithThisDestination)
                .HasForeignKey(p => p.DestinationMachineId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // property types
            modelBuilder.Entity<Courier>()
                .Property(c => c.Username)
                .HasColumnType("varchar(50)")
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

            modelBuilder.Entity<Shipment>()
                .Property(p => p.Title)
                .HasColumnType("varchar(50)")
                .IsRequired();
            modelBuilder.Entity<Shipment>()
                .Property(p => p.Description)
                .HasColumnType("varchar(1000)")
                .IsRequired(false);
            modelBuilder.Entity<Shipment>()
                .Property(p => p.Status)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
