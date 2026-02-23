using Microsoft.AspNetCore.Mvc;
using PD411_Shop.Data;

namespace PD411_Shop.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var products = _context.Products.AsEnumerable();
            return View(products);
        }
    }
}
