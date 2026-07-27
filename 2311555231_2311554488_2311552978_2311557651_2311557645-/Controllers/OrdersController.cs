using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2311555231_2311554488_2311552978_2311557651_2311557645_.Data;
using _2311555231_2311554488_2311552978_2311557651_2311557645_.Models;

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
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Products = _context.Products.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string customerName, int productId, int quantity)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null || quantity > product.StockQuantity)
                {
                    ModelState.AddModelError("", $"Số lượng vượt quá tồn kho (còn lại: {product?.StockQuantity ?? 0})");
                    ViewBag.Products = _context.Products.ToList();
                    return View();
                }

                var unitPrice = product.Price;
                var totalAmount = quantity * unitPrice;

                var order = new Order { CustomerName = customerName, OrderDate = DateTime.Now, TotalAmount = totalAmount };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                _context.OrderDetails.Add(new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = unitPrice
                });

                product.StockQuantity -= quantity;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", "Lỗi: " + ex.Message);
                ViewBag.Products = _context.Products.ToList();
                return View();
            }
        }
    }
}
