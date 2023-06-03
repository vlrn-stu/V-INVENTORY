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
    }
}