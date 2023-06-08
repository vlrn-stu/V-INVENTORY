using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VULTIME.VINV.Common.Models;
using VULTIME.VINV.Common.DataContracts;
using VULTIME.VINV.API.DB;

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

        [HttpGet("ImagesForItem/{id}")]
        public async Task<IActionResult> GetImages(Guid id)
        {
            try
            {
                var images = await _dbContext.InventoryItemImages.Where(i => i.InventoryItemId == id).ToListAsync();

                if (images == null || !images.Any())
                {
                    return NotFound();
                }

                return Ok(images);
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