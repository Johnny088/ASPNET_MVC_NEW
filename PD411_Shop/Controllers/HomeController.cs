
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PD411_Shop.Data;
using PD411_Shop.Models;
using PD411_Shop.Repositories;
using PD411_Shop.Services;
using PD411_Shop.ViewModels;
using System.Diagnostics;
using System.Text.Json;
namespace PD411_Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductRepository _productRepository;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, ProductRepository productRepository)
        {
            _logger = logger;
            _context = context;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index(int? category, [FromQuery]PaginationVM pagination) //category as name here must be equil to the name of asp-route from Index.cshtml
        {

            List<CategoryModel> categories = _context.Categories.ToList();


            var homeVM = new HomeVM
            {
                Products = await _productRepository.GetAllAsync(pagination, category),
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
