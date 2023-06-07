using V_INVENTORY.MODEL.DataContracts;
using V_INVENTORY.MODEL.Models;

namespace V_INVENTORY.MODEL.Helpers
{
    public static class ModelHelper
    {
        public static InventoryItemTO ConvertModelToTO(InventoryItem model)
        {
            return new InventoryItemTO
            {
                Id = model.Id,
                Name = model.Name,
                Status = model.Status,
                Description = model.Description,
                Quantity = model.Quantity,
                LocationId = model.LocationId,
                OriginalPrice = model.OriginalPrice,
                BuyDate = model.BuyDate
            };
        }
    }
}