using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using V_INVENTORY_API.DB;
using VCS.V_INVENTORY.MODEL.DataContracts;
using VCS.V_INVENTORY.MODEL.Models;

namespace V_INVENTORY_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemController : ControllerBase
    {
        private readonly InventoryDBContext _dbContext;

        public InventoryItemController(InventoryDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInventoryItem([FromBody] InventoryItemTO inventoryItemTO)
        {
            try
            {
                var location = await _dbContext.ItemLocations.FindAsync(inventoryItemTO.LocationId);
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
                    SellPrice = inventoryItemTO.SellPrice,
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventoryItem(Guid id)
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
    }
}