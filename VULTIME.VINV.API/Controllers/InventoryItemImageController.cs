using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VULTIME.VINV.API.DB;
using VULTIME.VINV.Common.DataContracts;
using VULTIME.VINV.Common.Models;

namespace VULTIME.VINV.API.Controllers
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
                InventoryItemImage? image = await _dbContext.InventoryItemImages.FindAsync(id);

                return image == null ? NotFound() : Ok(image);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("ImagesForItem/{id}")]
        public async Task<IActionResult> GetImages(Guid id)
        {
            try
            {
                List<InventoryItemImage> images = await _dbContext.InventoryItemImages.Where(i => i.InventoryItemId == id).ToListAsync();

                return images == null || !images.Any() ? NotFound() : Ok(images);
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
                InventoryItem? inventoryItem = await _dbContext.InventoryItems.FindAsync(imageTO.InventoryItemId);
                if (inventoryItem == null)
                {
                    return BadRequest("Invalid inventory item ID");
                }

                if (imageTO.ImageData == null || imageTO.ImageData.Length == 0)
                {
                    return BadRequest("Image data cannot be null or empty");
                }

                InventoryItemImage image = new()
                {
                    Id = Guid.NewGuid(),
                    ImageData = imageTO.ImageData,
                    InventoryItemId = imageTO.InventoryItemId
                };

                _ = _dbContext.InventoryItemImages.Add(image);
                _ = await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetImage), new { id = image.Id }, image);
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
                InventoryItemImage? image = await _dbContext.InventoryItemImages.FindAsync(id);
                if (image == null)
                {
                    return NotFound("Image not found");
                }

                _ = _dbContext.InventoryItemImages.Remove(image);
                _ = await _dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}