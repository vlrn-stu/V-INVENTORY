﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VULTIME.VINV.Common.Models;
using VULTIME.VINV.Common.DataContracts;
using VULTIME.VINV.API.DB;

namespace VULTIME.VINV.API.Controllers
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
        public async Task<IActionResult> GetInventoryItem([FromRoute] Guid id)
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

        [HttpGet("{id}/Minimal")]
        public async Task<IActionResult> GetInventoryItemMinimal([FromRoute] Guid id)
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

        [HttpGet("ByLocation/{locationId}")]
        public async Task<IActionResult> GetInventoryItemsByLocation([FromRoute] Guid locationId)
        {
            try
            {
                var location = await _dbContext.InventoryItemLocations.FindAsync(locationId);
                if (location == null)
                {
                    return NotFound("Location not found");
                }
                var inventoryItems = await _dbContext.InventoryItems
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
                var location = await _dbContext.InventoryItemLocations.FindAsync(locationId);
                if (location == null)
                {
                    return NotFound("Location not found");
                }
                var inventoryItems = await _dbContext.InventoryItems
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
                var location = await _dbContext.InventoryItemLocations.FindAsync(inventoryItemTO.LocationId);
                if (location == null)
                {
                    return NotFound("Location not found");
                }

                var inventoryItem = new InventoryItem
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

                _dbContext.InventoryItems.Add(inventoryItem);
                await _dbContext.SaveChangesAsync();

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

                inventoryItem.Name = inventoryItemTO.Name ?? throw new ArgumentNullException(nameof(inventoryItemTO.Name));
                inventoryItem.Status = inventoryItemTO.Status;
                inventoryItem.Description = inventoryItemTO.Description ?? throw new ArgumentNullException(nameof(inventoryItemTO.Description));
                inventoryItem.LocationId = inventoryItemTO.LocationId ?? throw new ArgumentNullException(nameof(inventoryItemTO.LocationId));
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
        public async Task<IActionResult> DeleteInventoryItem([FromRoute] Guid id)
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