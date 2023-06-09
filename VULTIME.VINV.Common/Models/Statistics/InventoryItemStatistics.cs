namespace VULTIME.VINV.Common.Models.Statistics
{
    public class InventoryItemStatistics
    {
        public int TotalItemCount { get; set; }
        public int TotalItemCountDeltaMonth { get; set; }
        public decimal TotalValue { get; set; }
        public decimal ValueDeltaMonth { get; set; }
    }
}