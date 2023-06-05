using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using V_INVENTORY_API.DB;

namespace V_INVENTORY_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InventoryItemOdataController : ControllerBase
	{
		private readonly InventoryDbContext _context;

		public InventoryItemOdataController(InventoryDbContext context)
		{
			_context = context;
		}

		[EnableQuery]
		public IActionResult Get()
		{
			return Ok(_context.InventoryItems);
		}
	}
}