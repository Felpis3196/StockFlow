using System.Text.Json.Serialization;

namespace StockFlow.Models
{
    public class ApiResponse
    {
        public string Message { get; set; }
        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }
    }
}
