using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourProject.Data;
using _2311555231_2311554488_2311552978_2311557651_2311557645_.ViewModels;

namespace _2311555231_2311554488_2311552978_2311557651_2311557645_.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly AppDbContext _context;

        public StatisticsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalRevenue = await _context.Orders
                .SumAsync(o => o.TotalAmount);

            var topProducts = await _context.OrderDetails
                .GroupBy(od => new
                {
                    od.ProductId,
                    od.Product!.Name
                })
                .Select(g => new TopProductViewModel
                {
                    ProductName = g.Key.Name,
                    TotalQuantitySold = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(p => p.TotalQuantitySold)
                .Take(3)
                .ToListAsync();

            return View(new StatisticsViewModel
            {
                TotalRevenue = totalRevenue,
                TopProducts = topProducts
            });
        }
    }
}
