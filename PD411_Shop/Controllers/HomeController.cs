
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PD411_Shop.Data;
using PD411_Shop.Models;
using PD411_Shop.Repositories;
using PD411_Shop.ViewModels;
using System.Diagnostics;
namespace PD411_Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
            
        }

        public IActionResult Index(int? category, [FromQuery]PaginationVM pagination) //category as name here must be equil to the name of asp-route from Index.cshtml
        {
            List<CategoryModel> categories = _context.Categories.ToList();
            IQueryable<ProductModel> products = _context.Products;
            if (category != null && categories.Any(c => c.Id == category))
            {
                products = products.Where(p => p.CategoryId == category);
            }
            // Pagination
            pagination.PageSize = pagination.PageSize < 1 ? 20 : pagination.PageSize;
            pagination.PageCount = (int)Math.Ceiling((double)products.Count() / pagination.PageSize);
            pagination.Page = pagination.Page < 1 || pagination.Page > pagination.PageCount ? 1 : pagination.Page;

            products = products
                .Skip(pagination.PageSize * (pagination.Page - 1))
                .Take(pagination.PageSize)
                .OrderBy(p => p.Id);

            var homeVM = new HomeVM
            {
                Products = products,
                Categories = categories,
                Pagination = pagination,
                CategoryId = category,
            };
            
            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AboutUs()
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
