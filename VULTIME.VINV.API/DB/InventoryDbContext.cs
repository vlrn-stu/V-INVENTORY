using Microsoft.EntityFrameworkCore;
using VULTIME.VINV.Common.Interfaces;
using VULTIME.VINV.Common.Models;

namespace VULTIME.VINV.API.DB
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
        }

        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<InventoryItemImage> InventoryItemImages { get; set; }
        public DbSet<InventoryItemLocation> InventoryItemLocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InventoryItem>()
                .HasMany(i => i.Images)
                .WithOne(img => img.InventoryItem)
                .HasForeignKey(img => img.InventoryItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InventoryItem>()
                .HasOne(i => i.Location)
                .WithMany()
                .HasForeignKey(i => i.LocationId);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is ITimestampedEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((ITimestampedEntity)entityEntry.Entity).UpdatedAt = DateTimeOffset.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    ((ITimestampedEntity)entityEntry.Entity).CreatedAt = DateTimeOffset.UtcNow;
                }
            }
        }
    }
}