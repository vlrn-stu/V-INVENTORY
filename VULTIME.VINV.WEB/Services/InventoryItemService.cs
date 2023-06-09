using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using VULTIME.VINV.Common.DataContracts;
using VULTIME.VINV.Common.Models;
using VULTIME.VINV.WEB.Shared.Models;

namespace VULTIME.VINV.WEB.Services
{
    public class InventoryItemService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public InventoryItemService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("InventoryAPI");
            _options.Converters.Add(new JsonStringEnumConverter());
        }

        public async Task<InventoryItem?> GetInventoryItem(Guid id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/InventoryItem/{id}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InventoryItem>(content, _options);
            }

            return null;
        }

        public async Task<(List<InventoryItem>? Items, int TotalCount)> SearchInventoryItemsByName(string nameFilter = "", int skip = 0, int top = 10)
        {
            string filterQuery = string.IsNullOrEmpty(nameFilter) ? "" : $"&$filter=Contains(Name,'{Regex.Replace(nameFilter, "[^a-zA-Z0-9_]+", "")}')";
            HttpResponseMessage response = await _httpClient.GetAsync($"odata/InventoryItems?$skip={skip}&$top={top}&$count=true{filterQuery}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                ODataResponse<InventoryItem>? result = JsonSerializer.Deserialize<ODataResponse<InventoryItem>>(content, _options);
                return (result?.Value, result?.Count ?? 0);
            }

            return (null, 0);
        }

        public class SearchResponse
        {
            public List<InventoryItem>? Items { get; set; }
            public int TotalCount { get; set; }
        }

        public async Task<InventoryItem?> CreateInventoryItem(InventoryItemTO item)
        {
            StringContent itemJson = new(
                JsonSerializer.Serialize(item),
                Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("api/InventoryItem", itemJson);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                return JsonSerializer.Deserialize<InventoryItem>(content, _options);
            }

            return null;
        }

        public async Task<InventoryItem?> UpdateInventoryItem(InventoryItemTO item)
        {
            StringContent itemJson = new(
                JsonSerializer.Serialize(item),
                Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync($"api/InventoryItem", itemJson);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InventoryItem>(content, _options);
            }

            return null;
        }

        public async Task<bool> DeleteInventoryItem(Guid id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/InventoryItem/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}