using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StockFlow.Models
{
    public class Item
    {
        [JsonPropertyName("id")]
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        [Required(ErrorMessage = "O nome do item é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres.")]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [JsonPropertyName("amount")]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade deve ser maior ou igual a 0.")]
        [Display(Name = "Quantidade")]
        public int Amount { get; set; }

        [JsonPropertyName("minAmount")]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade mínima deve ser maior ou igual a 0.")]
        [Display(Name = "Quantidade Mínima")]
        public int MinAmount { get; set; }

        [JsonPropertyName("category")]
        [Required(ErrorMessage = "A categoria é obrigatória.")]
        [StringLength(50, ErrorMessage = "A categoria não pode ter mais de 50 caracteres.")]
        [Display(Name = "Categoria")]
        public string Category { get; set; }

        [JsonPropertyName("supplier")]
        [Required(ErrorMessage = "O fornecedor é obrigatório.")]
        [StringLength(100, ErrorMessage = "O fornecedor não pode ter mais de 100 caracteres.")]
        [Display(Name = "Fornecedor")]
        public string Supplier { get; set; }

        [JsonPropertyName("price")]
        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que 0.")]
        [Display(Name = "Preço")]
        public string Price { get; set; }

        public decimal PriceAsDecimal => decimal.TryParse(Price, out var result) ? result : 0;
    }
}
