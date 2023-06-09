using System.Text.Json;
using VULTIME.VINV.Common.Models.Statistics;

namespace VULTIME.VINV.WEB.Services
{
    public class InventoryItemStatisticsService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public InventoryItemStatisticsService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("InventoryAPI");
        }

        public async Task<InventoryItemStatistics?> GetInventoryItemStatistics()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/InventoryItemStatistics");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InventoryItemStatistics>(content, _options);
            }

            return null;
        }
    }
}