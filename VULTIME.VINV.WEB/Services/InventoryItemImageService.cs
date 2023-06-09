using System.Text;
using System.Text.Json;
using VULTIME.VINV.Common.DataContracts;
using VULTIME.VINV.Common.Models;

namespace VULTIME.VINV.WEB.Services
{
    public class InventoryItemImageService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public InventoryItemImageService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("InventoryAPI");
        }

        public async Task<InventoryItemImage?> GetInventoryItemImage(Guid id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/InventoryItemImage/{id}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InventoryItemImage>(content, _options);
            }

            return null;
        }

        public async Task<List<InventoryItemImage>?> GetImagesForItem(Guid itemId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/InventoryItemImage/ImagesForItem/{itemId}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<InventoryItemImage>>(content, _options);
            }

            return null;
        }

        public async Task<InventoryItemImage?> CreateInventoryItemImage(InventoryItemImageTO itemImageTO)
        {
            StringContent itemImageJson = new(
                JsonSerializer.Serialize(itemImageTO),
                Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("api/InventoryItemImage", itemImageJson);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InventoryItemImage>(content, _options);
            }

            return null;
        }

        public async Task<bool> DeleteInventoryItemImage(Guid id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/InventoryItemImage/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}