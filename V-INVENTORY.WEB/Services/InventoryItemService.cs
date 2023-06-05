using System.Text;
using System.Text.Json;
using V_INVENTORY.MODEL.DataContracts;
using V_INVENTORY.MODEL.Models;

namespace Services
{
    public class InventoryItemService
    {
        private readonly HttpClient _httpClient;

        public InventoryItemService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("InventoryAPI");
        }

        public async Task<InventoryItem?> GetInventoryItem(Guid id)
        {
            var response = await _httpClient.GetAsync($"InventoryItem/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InventoryItem>(content);
            }

            return null;
        }

        public async Task<InventoryItem?> CreateInventoryItem(InventoryItemTO item)
        {
            var itemJson = new StringContent(
                JsonSerializer.Serialize(item),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("InventoryItem", itemJson);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InventoryItem>(content);
            }

            return null;
        }

        public async Task<InventoryItem?> UpdateInventoryItem(InventoryItemTO item)
        {
            var itemJson = new StringContent(
                JsonSerializer.Serialize(item),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PutAsync($"InventoryItem/{item.Id}", itemJson);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InventoryItem>(content);
            }

            return null;
        }

        public async Task<bool> DeleteInventoryItem(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"InventoryItem/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}