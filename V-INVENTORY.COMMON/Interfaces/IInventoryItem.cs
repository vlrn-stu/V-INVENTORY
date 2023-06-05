using V_INVENTORY.MODEL.Models;

namespace V_INVENTORY.MODEL.Interfaces
{
    public interface IInventoryItem : ITimestampedEntity
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        ICollection<InventoryItemImage> Images { get; set; }
        Guid LocationId { get; set; }
        InventoryItemLocation Location { get; set; }
        int Quantity { get; set; }
        decimal OriginalPrice { get; set; }
        DateOnly BuyDate { get; set; }
    }
}