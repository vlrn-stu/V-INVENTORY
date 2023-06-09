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
                await CalculateStatisticsAsync();
            });
        }

        public async Task<InventoryItemStatistics?> GetInventoryItemStatisticsAsync(bool forceRefresh = false)
        {
            if (!forceRefresh && inventoryItemStatictics != null)
            {
                return inventoryItemStatictics;
            }

            if (await CalculateStatisticsAsync())
            {
                return inventoryItemStatictics;
            }
            else
            {
                return null;
            }
        }

        private async Task<bool> CalculateStatisticsAsync()
        {
            try
            {
                using var _dbContext = _contextFactory.CreateDbContext();
                var now = DateTimeOffset.UtcNow;
                var oneMonthAgo = now.AddMonths(-1);

                var newStatistics = new InventoryItemStatistics
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