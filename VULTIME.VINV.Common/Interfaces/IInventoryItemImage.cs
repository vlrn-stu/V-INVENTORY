using VULTIME.VINV.Common.Models;

namespace VULTIME.VINV.Common.Interfaces
{
    public interface IInventoryItemImage
    {
        Guid Id { get; set; }
        byte[] ImageData { get; set; }
        Guid InventoryItemId { get; set; }
        InventoryItem InventoryItem { get; set; }
    }
}