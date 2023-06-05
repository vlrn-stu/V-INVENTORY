using V_INVENTORY.MODEL.Models;

namespace V_INVENTORY.MODEL.Interfaces
{
    public interface IInventoryItemImage
    {
        Guid Id { get; set; }
        byte[] ImageData { get; set; }
        Guid InventoryItemId { get; set; }
        InventoryItem InventoryItem { get; set; }
    }
}