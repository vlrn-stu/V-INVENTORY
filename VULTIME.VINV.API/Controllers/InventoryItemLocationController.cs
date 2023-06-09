using Microsoft.AspNetCore.Mvc;
using VULTIME.VINV.API.DB;
using VULTIME.VINV.Common.DataContracts;
using VULTIME.VINV.Common.Models;

namespace VULTIME.VINV.API.Controllers
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
                InventoryItemLocation? location = await _dbContext.InventoryItemLocations.FindAsync(id);

                return location == null ? NotFound() : Ok(location);
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
                InventoryItemLocation location = new()
                {
                    Id = Guid.NewGuid(),
                    Name = locationTO.Name ?? throw new ArgumentNullException(locationTO.Name)
                };

                _ = _dbContext.InventoryItemLocations.Add(location);
                _ = await _dbContext.SaveChangesAsync();

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
                InventoryItemLocation? location = await _dbContext.InventoryItemLocations.FindAsync(locationTO.Id);
                if (location == null)
                {
                    return NotFound("Location not found");
                }

                location.Name = locationTO.Name ?? throw new ArgumentNullException(locationTO.Name);
                _ = _dbContext.InventoryItemLocations.Update(location);
                _ = await _dbContext.SaveChangesAsync();

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
                InventoryItemLocation? location = await _dbContext.InventoryItemLocations.FindAsync(id);
                if (location == null)
                {
                    return NotFound("Location not found");
                }

                _ = _dbContext.InventoryItemLocations.Remove(location);
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