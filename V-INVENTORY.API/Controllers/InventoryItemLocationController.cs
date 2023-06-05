using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using V_INVENTORY.MODEL.DataContracts;
using V_INVENTORY.MODEL.Models;
using V_INVENTORY_API.DB;

namespace V_INVENTORY_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemLocationController : ControllerBase
    {
        private readonly InventoryDbContext _dbContext;

        public InventoryItemLocationController(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocation(Guid id)
        {
            try
            {
                var location = await _dbContext.InventoryItemLocations.FindAsync(id);

                if (location == null)
                {
                    return NotFound();
                }

                return Ok(location);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateLocation([FromBody] InventoryItemLocationTO locationTO)
        {
            try
            {
                var location = new InventoryItemLocation
                {
                    Id = Guid.NewGuid(),
                    Name = locationTO.Name ?? throw new ArgumentNullException(locationTO.Name)
                };

                _dbContext.InventoryItemLocations.Add(location);
                await _dbContext.SaveChangesAsync();

                return Ok(location);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLocation([FromBody] InventoryItemLocationTO locationTO)
        {
            try
            {
                var location = await _dbContext.InventoryItemLocations.FindAsync(locationTO.Id);
                if (location == null)
                {
                    return NotFound("Location not found");
                }

                location.Name = locationTO.Name ?? throw new ArgumentNullException(locationTO.Name);
                _dbContext.InventoryItemLocations.Update(location);
                await _dbContext.SaveChangesAsync();

                return Ok(location);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(Guid id)
        {
            try
            {
                var location = await _dbContext.InventoryItemLocations.FindAsync(id);
                if (location == null)
                {
                    return NotFound("Location not found");
                }

                _dbContext.InventoryItemLocations.Remove(location);
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