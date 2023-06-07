using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using V_INVENTORY.MODEL.Models;
using V_INVENTORY_API.DB;

namespace V_INVENTORY_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemODataController : ControllerBase
    {
        private readonly InventoryDbContext _context;

        public InventoryItemODataController(InventoryDbContext context)
        {
            _context = context;
        }

        [EnableQuery]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.InventoryItems);
        }

        [EnableQuery]
        [HttpGet("WithLocation")]
        public IActionResult GetWithLocation(ODataQueryOptions<InventoryItem> options)
        {
            var itemsQuery = _context.InventoryItems.Include(i => i.Location).AsQueryable();

            if (options.Count?.Value == true)
            {
                Request.ODataFeature().TotalCount = options.ApplyTo(itemsQuery).Cast<InventoryItem>().Count();
            }

            var items = options.ApplyTo(itemsQuery) as IQueryable<InventoryItem>;

            return Ok(items);
        }
    }
}