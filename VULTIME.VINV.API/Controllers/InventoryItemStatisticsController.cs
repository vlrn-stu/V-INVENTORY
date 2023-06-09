using Microsoft.AspNetCore.Mvc;
using VULTIME.VINV.API.Statisctics;
using VULTIME.VINV.Common.Models.Statistics;

namespace VULTIME.VINV.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemStatisticsController : ControllerBase
    {
        private readonly InventoryItemStatisticsProvider _inventoryItemStatisticsProvider;

        public InventoryItemStatisticsController(InventoryItemStatisticsProvider inventoryItemStatisticsProvider)
        {
            _inventoryItemStatisticsProvider = inventoryItemStatisticsProvider;
        }

        [HttpGet]
        public async Task<ActionResult<InventoryItemStatistics>> GetInventoryItemStatisticsAsync([FromQuery] bool forceRefresh = false)
        {
            InventoryItemStatistics? statistics = await _inventoryItemStatisticsProvider.GetInventoryItemStatisticsAsync(forceRefresh);

            return statistics == null ? (ActionResult<InventoryItemStatistics>)NotFound() : (ActionResult<InventoryItemStatistics>)statistics;
        }
    }
}