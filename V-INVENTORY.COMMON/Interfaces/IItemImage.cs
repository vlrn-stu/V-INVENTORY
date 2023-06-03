using VCS.V_INVENTORY.MODEL.Models;

namespace VCS.V_INVENTORY.MODEL.Interfaces
{
    public interface IItemImage
    {
        Guid Id { get; set; }
        byte[] ImageData { get; set; }
        Guid InventoryItemId { get; set; }
        InventoryItem InventoryItem { get; set; }
    }
}