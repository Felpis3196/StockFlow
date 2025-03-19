using System.Text;
using System.Text.Json;
using StockFlow.Models;

namespace StockFlow.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:3000/items";

        public ItemRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {      
            var response = await _httpClient.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine(json);

            var parsedJson = JsonSerializer.Deserialize<ApiResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

            return parsedJson.Items; 
        }

        public async Task<Item?> GetByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Item>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task AddAsync(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "O item não pode ser nulo.");
            }

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(item, options);
            Console.WriteLine("JSON enviado: " + json);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(BaseUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erro ao adicionar item: {response.StatusCode} - {errorMessage}");
            }
        }


        public async Task UpdateAsync(Item item)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(item, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{BaseUrl}/{item.Id}", content);
            response.EnsureSuccessStatusCode();
        }


        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
