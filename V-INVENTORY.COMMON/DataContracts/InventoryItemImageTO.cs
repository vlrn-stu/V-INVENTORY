namespace V_INVENTORY.MODEL.DataContracts
{
    public class InventoryItemImageTO
    {
        public byte[]? ImageData { get; set; }
        public Guid InventoryItemId { get; set; }
    }
}