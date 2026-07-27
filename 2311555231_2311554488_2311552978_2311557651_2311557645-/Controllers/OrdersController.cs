using Microsoft.AspNetCore.Mvc;
using YourProject.Data;

namespace _2311555231_2311554488_2311552978_2311557651_2311557645_.Controllers
{
    //public class OrdersController : Controller
    //{
    //    public IActionResult Index()
    //    {
    //        return View();
    //    }
    //}

    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;
        public OrdersController(AppDbContext context) { _context = context; }

        public async Task<IActionResult> Index(string? customerName, decimal? fromPrice, decimal? toPrice)
        {
            var query = _context.Orders.AsQueryable();

            if (!string.IsNullOrEmpty(customerName))
                query = query.Where(o => o.CustomerName.Contains(customerName));
            if (fromPrice.HasValue)
                query = query.Where(o => o.TotalAmount >= fromPrice.Value);
            if (toPrice.HasValue)
                query = query.Where(o => o.TotalAmount <= toPrice.Value);

            var orders = await query
                .Select(o => new OrderListViewModel
                {
                    Id = o.Id,
                    CustomerName = o.CustomerName,
                    OrderDate = o.OrderDate,
                    DistinctProductCount = o.OrderDetails.Select(od => od.ProductId).Distinct().Count()
                })
                .ToListAsync();

            return View(orders);
        }
    }
}
