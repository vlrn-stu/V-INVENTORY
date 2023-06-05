using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using V_INVENTORY.MODEL.Interfaces;

namespace V_INVENTORY.MODEL.Models
{
    public class InventoryItem : IInventoryItem
    {
        [Key]
        public Guid Id { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<InventoryItemImage> Images { get; set; }

        [Required]
        [ForeignKey("ItemLocation")]
        public Guid LocationId { get; set; }

        public InventoryItemLocation Location { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalPrice { get; set; }

        public DateOnly BuyDate { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}