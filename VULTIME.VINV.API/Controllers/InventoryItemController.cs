using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VULTIME.Core.Data.Enums;
using VULTIME.Core.Data.Notifications;
using VULTIME.VINV.API.DB;
using VULTIME.VINV.Common.DataContracts;
using VULTIME.VINV.Common.Models;

namespace VULTIME.VINV.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemController : ControllerBase
    {
        private readonly InventoryDbContext _dbContext;
        private readonly INotificationManager _notificationManager;

        public InventoryItemController(InventoryDbContext dbContext, INotificationManager notificationManager)
        {
            _dbContext = dbContext;
            _notificationManager = notificationManager;
        }

#if DEBUG

        [HttpPost("GenerateRandom/{count}")]
        public async Task<IActionResult> GenerateRandomInventoryItems([FromRoute] int count)
        {
            try
            {
                List<InventoryItemLocation> locations = await _dbContext.InventoryItemLocations.ToListAsync();
                if (!locations.Any())
                {
                    return BadRequest("At least one location is required to generate random items.");
                }

                Random random = new();
                List<InventoryItem> inventoryItems = new();

                for (int i = 0; i < count; i++)
                {
                    InventoryItem item = new()
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Item {Guid.NewGuid()}",
                        Status = Enum.GetValues(typeof(InventoryItemStatus))
                                     .Cast<InventoryItemStatus>()
                                     .OrderBy(_ => random.Next())
                                     .FirstOrDefault(),
                        Description = $"Description {Guid.NewGuid()}",
                        LocationId = locations[random.Next(0, locations.Count - 1)].Id,
                        Quantity = random.Next(1, 101),
                        OriginalPrice = (decimal)random.NextDouble() * 100,
                        BuyDate = DateTimeOffset.Now.AddDays(-random.Next(0, 365)).ToOffset(TimeSpan.Zero)
                    };

                    inventoryItems.Add(item);
                }

                await _dbContext.AddRangeAsync(inventoryItems);
                _ = await _dbContext.SaveChangesAsync();

                _notificationManager.Notify(typeof(InventoryItem), EntityOperation.Create, Guid.Empty);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

#endif

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventoryItem([FromRoute] Guid id)
        {
            try
            {
                InventoryItem? inventoryItem = await _dbContext.InventoryItems
                    .Include(i => i.Images)
                    .Include(i => i.Location)
                    .FirstOrDefaultAsync(i => i.Id == id);

                return inventoryItem == null ? NotFound() : Ok(inventoryItem);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/Minimal")]
        public async Task<IActionResult> GetInventoryItemMinimal([FromRoute] Guid id)
        {
            try
            {
                InventoryItem? inventoryItem = await _dbContext.InventoryItems.FindAsync(id);

                return inventoryItem == null ? NotFound() : Ok(inventoryItem);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("ByLocation/{locationId}")]
        public async Task<IActionResult> GetInventoryItemsByLocation([FromRoute] Guid locationId)
        {
            try
            {
                InventoryItemLocation? location = await _dbContext.InventoryItemLocations.FindAsync(locationId);
                if (location == null)
                {
                    return NotFound("Location not found");
                }
                List<InventoryItem> inventoryItems = await _dbContext.InventoryItems
                    .Include(i => i.Images)
                    .Include(i => i.Location)
                    .Where(i => i.LocationId == locationId)
                    .ToListAsync();
                return Ok(inventoryItems);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("ByLocation/{locationId}/Minimal")]
        public async Task<IActionResult> GetInventoryItemsByLocationMinimal([FromRoute] Guid locationId)
        {
            try
            {
                InventoryItemLocation? location = await _dbContext.InventoryItemLocations.FindAsync(locationId);
                if (location == null)
                {
                    return NotFound("Location not found");
                }
                List<InventoryItem> inventoryItems = await _dbContext.InventoryItems
                    .Where(i => i.LocationId == locationId)
                    .ToListAsync();
                return Ok(inventoryItems);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateInventoryItem([FromBody] InventoryItemTO inventoryItemTO)
        {
            try
            {
                InventoryItemLocation? location = await _dbContext.InventoryItemLocations.FindAsync(inventoryItemTO.LocationId);
                if (location == null)
                {
                    return NotFound("Location not found");
                }

                InventoryItem inventoryItem = new()
                {
                    Id = Guid.NewGuid(),
                    Name = inventoryItemTO.Name ?? throw new ArgumentNullException(nameof(inventoryItemTO.Name)),
                    Status = inventoryItemTO.Status,
                    Description = inventoryItemTO.Description ?? throw new ArgumentNullException(nameof(inventoryItemTO.Description)),
                    LocationId = inventoryItemTO.LocationId ?? throw new ArgumentNullException(nameof(inventoryItemTO.LocationId)),
                    Quantity = inventoryItemTO.Quantity < 0 ? throw new ArgumentException("Quantity cannot be negative") : inventoryItemTO.Quantity,
                    OriginalPrice = inventoryItemTO.OriginalPrice,
                    BuyDate = inventoryItemTO.BuyDate.ToOffset(TimeSpan.Zero)
                };

                _ = _dbContext.InventoryItems.Add(inventoryItem);
                _ = await _dbContext.SaveChangesAsync();

                _notificationManager.Notify(typeof(InventoryItem), EntityOperation.Create, inventoryItem.Id);

                return CreatedAtAction(nameof(GetInventoryItem), new { id = inventoryItem.Id }, inventoryItem);
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
                InventoryItemLocation? location = await _dbContext.InventoryItemLocations.FindAsync(inventoryItemTO.LocationId);
                if (location == null)
                {
                    return NotFound("Location not found");
                }

                InventoryItem? inventoryItem = await _dbContext.InventoryItems.FindAsync(inventoryItemTO.Id);
                if (inventoryItem == null)
                {
                    return NotFound("Inventory item not found");
                }

                inventoryItem.Name = inventoryItemTO.Name ?? throw new ArgumentNullException(nameof(inventoryItemTO.Name));
                inventoryItem.Status = inventoryItemTO.Status;
                inventoryItem.Description = inventoryItemTO.Description ?? throw new ArgumentNullException(nameof(inventoryItemTO.Description));
                inventoryItem.LocationId = inventoryItemTO.LocationId ?? throw new ArgumentNullException(nameof(inventoryItemTO.LocationId));
                inventoryItem.Quantity = inventoryItemTO.Quantity < 0 ? throw new ArgumentException("Quantity cannot be negative") : inventoryItemTO.Quantity;
                inventoryItem.OriginalPrice = inventoryItemTO.OriginalPrice;
                inventoryItem.BuyDate = inventoryItemTO.BuyDate;

                _ = _dbContext.InventoryItems.Update(inventoryItem);
                _ = await _dbContext.SaveChangesAsync();

                _notificationManager.Notify(typeof(InventoryItem), EntityOperation.Update, inventoryItem.Id);

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
        public async Task<IActionResult> DeleteInventoryItem([FromRoute] Guid id)
        {
            try
            {
                InventoryItem? inventoryItem = await _dbContext.InventoryItems.FindAsync(id);
                if (inventoryItem == null)
                {
                    return NotFound("Inventory item not found");
                }

                _ = _dbContext.InventoryItems.Remove(inventoryItem);
                _ = await _dbContext.SaveChangesAsync();

                _notificationManager.Notify(typeof(InventoryItem), EntityOperation.Delete, inventoryItem.Id);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}