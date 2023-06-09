using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using VULTIME.VINV.API.DB;

namespace VULTIME.VINV.API.Controllers
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