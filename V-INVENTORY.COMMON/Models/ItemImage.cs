using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VCS.V_INVENTORY.MODEL.Interfaces;

namespace VCS.V_INVENTORY.MODEL.Models
{
    public class ItemImage : IItemImage
    {
        [Key]
        public Guid Id { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Required]
        public byte[] ImageData { get; set; }

        [Required]
        [ForeignKey("InventoryItem")]
        public Guid InventoryItemId { get; set; }

        public InventoryItem InventoryItem { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}