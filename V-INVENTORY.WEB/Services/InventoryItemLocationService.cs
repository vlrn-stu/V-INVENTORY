using System.Text;
using System.Text.Json;
using V_INVENTORY.MODEL.Models;
using V_INVENTORY.WEB.Shared.Models;

namespace Services
{
    public class InventoryItemLocationService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public InventoryItemLocationService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("InventoryAPI");
        }

        public async Task<IEnumerable<InventoryItemLocation>> GetAllInventoryItemLocations()
        {
            var response = await _httpClient.GetAsync("odata/InventoryItemLocations");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ODataResponse<InventoryItemLocation>>(content, _options);
                return result?.Value ?? new List<InventoryItemLocation>();
            }

            return new List<InventoryItemLocation>();
        }

        public async Task<InventoryItemLocation?> GetInventoryItemLocation(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/InventoryItemLocation/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InventoryItemLocation>(content, _options);
            }

            return null;
        }

        public async Task<InventoryItemLocation?> CreateInventoryItemLocation(InventoryItemLocation itemLocation)
        {
            var itemLocationJson = new StringContent(
                JsonSerializer.Serialize(itemLocation),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("api/inventoryitemlocation", itemLocationJson);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InventoryItemLocation>(content, _options);
            }

            return null;
        }

        public async Task<InventoryItemLocation?> UpdateInventoryItemLocation(InventoryItemLocation itemLocation)
        {
            var itemLocationJson = new StringContent(
                JsonSerializer.Serialize(itemLocation),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PutAsync($"api/inventoryitemlocation/{itemLocation.Id}", itemLocationJson);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InventoryItemLocation>(content, _options);
            }

            return null;
        }

        public async Task<bool> DeleteInventoryItemLocation(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/inventoryitemlocation/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}