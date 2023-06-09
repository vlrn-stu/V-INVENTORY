using Microsoft.EntityFrameworkCore;
using VULTIME.Core.Data.Enums;
using VULTIME.Core.Data.Notifications;
using VULTIME.VINV.API.DB;
using VULTIME.VINV.Common.Models;
using VULTIME.VINV.Common.Models.Statistics;

namespace VULTIME.VINV.API.Statisctics
{
    public class InventoryItemStatisticsProvider
    {
        private readonly IDbContextFactory<InventoryDbContext> _contextFactory;
        private readonly INotificationManager _notificationManager;
        private InventoryItemStatistics? inventoryItemStatictics = null;

        public InventoryItemStatisticsProvider(IDbContextFactory<InventoryDbContext> contextFactor, INotificationManager notificationManager)
        {
            _contextFactory = contextFactor;
            _notificationManager = notificationManager;

            _notificationManager.Subscribe(typeof(InventoryItem),
                new List<EntityOperation> { EntityOperation.Create, EntityOperation.Update, EntityOperation.Delete },
                async args =>
            {
                _ = await CalculateStatisticsAsync();
            });
        }

        public async Task<InventoryItemStatistics?> GetInventoryItemStatisticsAsync(bool forceRefresh = false)
        {
            return !forceRefresh && inventoryItemStatictics != null
                ? inventoryItemStatictics
                : await CalculateStatisticsAsync() ? inventoryItemStatictics : null;
        }

        private async Task<bool> CalculateStatisticsAsync()
        {
            try
            {
                using InventoryDbContext _dbContext = _contextFactory.CreateDbContext();
                DateTimeOffset now = DateTimeOffset.UtcNow;
                DateTimeOffset oneMonthAgo = now.AddMonths(-1);

                InventoryItemStatistics newStatistics = new()
                {
                    TotalItemCount = await _dbContext.InventoryItems.CountAsync(),

                    TotalItemCountDeltaMonth = await _dbContext.InventoryItems.CountAsync() -
                        await _dbContext.InventoryItems.Where(item => item.BuyDate <= oneMonthAgo).CountAsync(),

                    TotalValue = await _dbContext.InventoryItems.SumAsync(item => item.OriginalPrice * item.Quantity),

                    ValueDeltaMonth = await _dbContext.InventoryItems.SumAsync(item => item.OriginalPrice * item.Quantity) -
                        await _dbContext.InventoryItems.Where(item => item.BuyDate <= oneMonthAgo)
                        .SumAsync(item => item.OriginalPrice * item.Quantity)
                };

                inventoryItemStatictics = newStatistics;
                return true;
            }
            catch (Exception)
            {
                inventoryItemStatictics = null;
                return false;
            }
        }
    }
}