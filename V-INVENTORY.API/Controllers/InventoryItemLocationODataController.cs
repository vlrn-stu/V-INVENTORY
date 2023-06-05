using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using V_INVENTORY_API.DB;

namespace V_INVENTORY_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemLocationODataController : ControllerBase
    {
        private readonly InventoryDbContext _context;

        public InventoryItemLocationODataController(InventoryDbContext context)
        {
            _context = context;
        }

        [EnableQuery]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.InventoryItemLocations);
        }
    }
}