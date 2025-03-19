using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StockFlow.Models;
using StockFlow.Models.ViewModels;

namespace StockFlow.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IItemService _itemService;

        public HomeController(ILogger<HomeController> logger, IItemService itemService)
        {
            _itemService = itemService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _itemService.GetAllAsync();
            var totalItems = items.Sum(i => i.Amount);

            var mostCommonSupplier = items
                .GroupBy(i => i.Supplier)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            var mostCommonCategory = items
                .GroupBy(i => i.Category)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            var totalSuppliers = items
                .Select(i => i.Supplier)
                .Distinct()
                .Count();

            var totalCategories = items
                .Select(i => i.Category)
                .Distinct()
                .Count();

            var supplierDistribution = items
                .GroupBy(i => i.Supplier)
                .ToDictionary(g => g.Key, g => Math.Round((g.Sum(i => i.Amount) / (double)totalItems) * 100, 2));

            var categoryDistribution = items
                .GroupBy(i => i.Category)
                .ToDictionary(g => g.Key, g => Math.Round((g.Sum(i => i.Amount) / (double)totalItems) * 100, 2));


            var dashboardData = new DashboardViewModel
            {
                MostExpensiveItem = items.OrderByDescending(i => i.PriceAsDecimal).FirstOrDefault(),
                CheapestItem = items.OrderBy(i => i.PriceAsDecimal).FirstOrDefault(),
                AveragePrice = items.Any() ? items.Average(i => i.PriceAsDecimal) : 0,
                TotalStockValue = items.Sum(i => i.PriceAsDecimal * i.Amount),
                TotalItems = items.Sum(i => i.Amount),
                MostCommonSupplier = mostCommonSupplier,
                MostCommonCategory = mostCommonCategory,
                TotalSuppliers = totalSuppliers,
                TotalCategories = totalCategories,
                SupplierDistribution = supplierDistribution,
                CategoryDistribution = categoryDistribution
            };

            return View(dashboardData);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
