using VCS.V_INVENTORY.MODEL.Models;

namespace VCS.V_INVENTORY.MODEL.Interfaces
{
    public interface IInventoryItem : ITimestampedEntity
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        ICollection<ItemImage> Images { get; set; }
        Guid LocationId { get; set; }
        ItemLocation Location { get; set; }
        int Quantity { get; set; }
        decimal OriginalPrice { get; set; }
        decimal SellPrice { get; set; }
        DateTime BuyDate { get; set; }
    }
}