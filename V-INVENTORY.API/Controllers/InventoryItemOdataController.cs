using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using V_INVENTORY_API.DB;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace V_INVENTORY_API.Controllers
{
    public class InventoryItemsController : ODataController
    {
        private readonly InventoryDbContext _context;

        public InventoryItemsController(InventoryDbContext context)
        {
            _context = context;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_context.InventoryItems.Include(i => i.Location).AsQueryable());
        }
    }
}