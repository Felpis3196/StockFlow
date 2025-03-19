namespace StockFlow.Models.ViewModels
{
    public class DashboardViewModel
    {
        public Item? MostExpensiveItem { get; set; }
        public Item? CheapestItem { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal TotalStockValue { get; set; }
        public int TotalItems { get; set; }
        public string? MostCommonSupplier { get; set; }
        public string? MostCommonCategory { get; set; }
        public int TotalSuppliers { get; set; }
        public int TotalCategories { get; set; }
        public Dictionary<string, double> SupplierDistribution { get; set; } = new();
        public Dictionary<string, double> CategoryDistribution { get; set; } = new();
    }
}
