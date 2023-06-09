using System.Text;
using System.Text.Json;
using VULTIME.VINV.Common.Models;
using VULTIME.VINV.WEB.Shared.Models;

namespace VULTIME.VINV.WEB.Services
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
            HttpResponseMessage response = await _httpClient.GetAsync("odata/InventoryItemLocations");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                ODataResponse<InventoryItemLocation>? result = JsonSerializer.Deserialize<ODataResponse<InventoryItemLocation>>(content, _options);
                return result?.Value ?? new List<InventoryItemLocation>();
            }

            return new List<InventoryItemLocation>();
        }

        public async Task<InventoryItemLocation?> GetInventoryItemLocation(Guid id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/InventoryItemLocation/{id}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InventoryItemLocation>(content, _options);
            }

            return null;
        }

        public async Task<InventoryItemLocation?> CreateInventoryItemLocation(InventoryItemLocation itemLocation)
        {
            StringContent itemLocationJson = new(
                JsonSerializer.Serialize(itemLocation),
                Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("api/inventoryitemlocation", itemLocationJson);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InventoryItemLocation>(content, _options);
            }

            return null;
        }

        public async Task<InventoryItemLocation?> UpdateInventoryItemLocation(InventoryItemLocation itemLocation)
        {
            StringContent itemLocationJson = new(
                JsonSerializer.Serialize(itemLocation),
                Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"api/inventoryitemlocation/{itemLocation.Id}", itemLocationJson);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InventoryItemLocation>(content, _options);
            }

            return null;
        }

        public async Task<bool> DeleteInventoryItemLocation(Guid id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/inventoryitemlocation/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}