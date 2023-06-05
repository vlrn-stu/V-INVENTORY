using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using V_INVENTORY.MODEL.DataContracts;
using V_INVENTORY_API.DB;
using V_INVENTORY.MODEL.Models;

namespace V_INVENTORY_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InventoryItemController : ControllerBase
	{
		private readonly InventoryDbContext _dbContext;

		public InventoryItemController(InventoryDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetInventoryItem(Guid id)
		{
			try
			{
				var inventoryItem = await _dbContext.InventoryItems
					.Include(i => i.Images)
					.Include(i => i.Location)
					.FirstOrDefaultAsync(i => i.Id == id);

				if (inventoryItem == null)
				{
					return NotFound();
				}

				return Ok(inventoryItem);
			}
			catch (Exception ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpGet("{id}/minimal")]
		public async Task<IActionResult> GetInventoryItemMinimal(Guid id)
		{
			try
			{
				var inventoryItem = await _dbContext.InventoryItems.FindAsync(id);

				if (inventoryItem == null)
				{
					return NotFound();
				}

				return Ok(inventoryItem);
			}
			catch (Exception ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> CreateInventoryItem([FromBody] InventoryItemTO inventoryItemTO)
		{
			try
			{
				var location = await _dbContext.InventoryItemLocations.FindAsync(inventoryItemTO.LocationId);
				if (location == null)
				{
					return NotFound("Location not found");
				}

				var inventoryItem = new InventoryItem
				{
					Id = Guid.NewGuid(),
					Name = inventoryItemTO.Name ?? throw new ArgumentNullException(inventoryItemTO.Name),
					Description = inventoryItemTO.Description ?? throw new ArgumentNullException(inventoryItemTO.Description),
					LocationId = inventoryItemTO.LocationId,
					Quantity = inventoryItemTO.Quantity < 0 ? throw new ArgumentException("Quantity cannot be negative") : inventoryItemTO.Quantity,
					OriginalPrice = inventoryItemTO.OriginalPrice,
					BuyDate = inventoryItemTO.BuyDate
				};

				_dbContext.InventoryItems.Add(inventoryItem);
				await _dbContext.SaveChangesAsync();

				return Ok(inventoryItem);
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		[HttpPut]
		public async Task<IActionResult> UpdateInventoryItem([FromBody] InventoryItemTO inventoryItemTO)
		{
			try
			{
				var location = await _dbContext.InventoryItemLocations.FindAsync(inventoryItemTO.LocationId);
				if (location == null)
				{
					return NotFound("Location not found");
				}

				var inventoryItem = await _dbContext.InventoryItems.FindAsync(inventoryItemTO.Id);
				if (inventoryItem == null)
				{
					return NotFound("Inventory item not found");
				}

				inventoryItem.Name = inventoryItemTO.Name ?? throw new ArgumentNullException(inventoryItemTO.Name);
				inventoryItem.Description = inventoryItemTO.Description ?? throw new ArgumentNullException(inventoryItemTO.Description);
				inventoryItem.LocationId = inventoryItemTO.LocationId;
				inventoryItem.Quantity = inventoryItemTO.Quantity < 0 ? throw new ArgumentException("Quantity cannot be negative") : inventoryItemTO.Quantity;
				inventoryItem.OriginalPrice = inventoryItemTO.OriginalPrice;
				inventoryItem.BuyDate = inventoryItemTO.BuyDate;

				_dbContext.InventoryItems.Update(inventoryItem);
				await _dbContext.SaveChangesAsync();

				return Ok(inventoryItem);
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteInventoryItem(Guid id)
		{
			try
			{
				var inventoryItem = await _dbContext.InventoryItems.FindAsync(id);
				if (inventoryItem == null)
				{
					return NotFound("Inventory item not found");
				}

				_dbContext.InventoryItems.Remove(inventoryItem);
				await _dbContext.SaveChangesAsync();

				return Ok();
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}
	}
}