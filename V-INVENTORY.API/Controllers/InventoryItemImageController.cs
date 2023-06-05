using Microsoft.AspNetCore.Mvc;
using V_INVENTORY.MODEL.DataContracts;
using V_INVENTORY.MODEL.Models;
using V_INVENTORY_API.DB;

namespace V_INVENTORY_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemImageController : ControllerBase
    {
        private readonly InventoryDbContext _dbContext;

        public InventoryItemImageController(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(Guid id)
        {
            try
            {
                var image = await _dbContext.InventoryItemImages.FindAsync(id);

                if (image == null)
                {
                    return NotFound();
                }

                return Ok(image);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateImage([FromBody] InventoryItemImageTO imageTO)
        {
            try
            {
                var inventoryItem = await _dbContext.InventoryItems.FindAsync(imageTO.InventoryItemId);
                if (inventoryItem == null)
                {
                    return BadRequest("Invalid inventory item ID");
                }

                if (imageTO.ImageData == null || imageTO.ImageData.Length == 0)
                {
                    return BadRequest("Image data cannot be null or empty");
                }

                var image = new InventoryItemImage
                {
                    Id = Guid.NewGuid(),
                    ImageData = imageTO.ImageData,
                    InventoryItemId = imageTO.InventoryItemId
                };

                _dbContext.InventoryItemImages.Add(image);
                await _dbContext.SaveChangesAsync();

                return Ok(image);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            try
            {
                var image = await _dbContext.InventoryItemImages.FindAsync(id);
                if (image == null)
                {
                    return NotFound("Image not found");
                }

                _dbContext.InventoryItemImages.Remove(image);
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