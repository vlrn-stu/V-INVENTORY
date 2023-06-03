using Microsoft.EntityFrameworkCore;
using VCS.V_INVENTORY.MODEL.Interfaces;
using VCS.V_INVENTORY.MODEL.Models;

namespace V_INVENTORY_API.DB
{
    public class InventoryDBContext : DbContext
    {
        public InventoryDBContext(DbContextOptions<InventoryDBContext> options) : base(options)
        {
        }

        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<ItemImage> ItemImages { get; set; }
        public DbSet<ItemLocation> ItemLocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InventoryItem>()
                .HasMany(i => i.Images)
                .WithOne(img => img.InventoryItem)
                .HasForeignKey(img => img.InventoryItemId);

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
                ((ITimestampedEntity)entityEntry.Entity).UpdatedAt = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((ITimestampedEntity)entityEntry.Entity).CreatedAt = DateTime.Now;
                }
            }
        }
    }
}