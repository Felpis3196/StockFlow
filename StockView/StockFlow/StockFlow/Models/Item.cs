using System.Text.Json.Serialization;

namespace StockFlow.Models
{
    public class Item
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("minAmount")]
        public int MinAmount { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("supplier")]
        public string Supplier { get; set; }
    }
}
