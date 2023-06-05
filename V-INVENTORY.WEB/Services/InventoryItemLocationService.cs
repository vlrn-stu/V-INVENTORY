using System.Text;
using System.Text.Json;
using V_INVENTORY.MODEL.Models;

namespace Services
{
	public class InventoryItemLocationService
	{
		private readonly HttpClient _httpClient;

		public InventoryItemLocationService(IHttpClientFactory clientFactory)
		{
			_httpClient = clientFactory.CreateClient("InventoryAPI");
		}

		public async Task<InventoryItemLocation?> GetInventoryItemLocation(Guid id)
		{
			var response = await _httpClient.GetAsync($"inventoryitemlocation/{id}");

			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				return JsonSerializer.Deserialize<InventoryItemLocation>(content);
			}

			return null;
		}

		public async Task<InventoryItemLocation?> CreateInventoryItemLocation(InventoryItemLocation itemLocation)
		{
			var itemLocationJson = new StringContent(
				JsonSerializer.Serialize(itemLocation),
				Encoding.UTF8,
				"application/json");

			var response = await _httpClient.PostAsync("inventoryitemlocation", itemLocationJson);

			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				return JsonSerializer.Deserialize<InventoryItemLocation>(content);
			}

			return null;
		}

		public async Task<InventoryItemLocation?> UpdateInventoryItemLocation(InventoryItemLocation itemLocation)
		{
			var itemLocationJson = new StringContent(
				JsonSerializer.Serialize(itemLocation),
				Encoding.UTF8,
				"application/json");

			var response = await _httpClient.PutAsync($"inventoryitemlocation/{itemLocation.Id}", itemLocationJson);

			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				return JsonSerializer.Deserialize<InventoryItemLocation>(content);
			}

			return null;
		}

		public async Task<bool> DeleteInventoryItemLocation(Guid id)
		{
			var response = await _httpClient.DeleteAsync($"inventoryitemlocation/{id}");

			return response.IsSuccessStatusCode;
		}
	}
}