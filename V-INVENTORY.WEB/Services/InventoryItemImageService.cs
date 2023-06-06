using Microsoft.AspNetCore.Components.Forms;
using SkiaSharp;
using System.Text;
using System.Text.Json;
using V_INVENTORY.MODEL.DataContracts;
using V_INVENTORY.MODEL.Models;

namespace Services
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
            var response = await _httpClient.GetAsync($"inventoryitemimage/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InventoryItemImage>(content, _options);
            }

            return null;
        }

        public async Task<InventoryItemImage?> CreateInventoryItemImage(InventoryItemImageTO itemImageTO)
        {
            var itemImageJson = new StringContent(
                JsonSerializer.Serialize(itemImageTO),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("inventoryitemimage", itemImageJson);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InventoryItemImage>(content, _options);
            }

            return null;
        }

        public async Task<bool> DeleteInventoryItemImage(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"inventoryitemimage/{id}");

            return response.IsSuccessStatusCode;
        }

        public static async Task<byte[]> ProcessImage(IBrowserFile file)
        {
            if (file.Size > 2 * 1024 * 1024)
            {
                throw new Exception("File size must be less than 2MB.");
            }

            await using var stream = file.OpenReadStream(file.Size);
            var bitmap = SKBitmap.Decode(stream);
            var image = SKImage.FromBitmap(bitmap);

            var data = image.Encode(SKEncodedImageFormat.Jpeg, 80);

            return data.ToArray();
        }
    }
}