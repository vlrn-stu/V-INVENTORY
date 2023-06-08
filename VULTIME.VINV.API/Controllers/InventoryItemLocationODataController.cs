using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using VULTIME.VINV.API.DB;

namespace VULTIME.VINV.API.Controllers
{
    public class InventoryItemLocationsController : ControllerBase
    {
        private readonly InventoryDbContext _context;

        public InventoryItemLocationsController(InventoryDbContext context)
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