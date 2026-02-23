using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PD411_Shop.Data;
using PD411_Shop.Models;
using PD411_Shop.Repositories;

namespace PD411_Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductRepository _productRepository;

        public HomeController(ILogger<HomeController> logger, ProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
            
        }

        public IActionResult Index()
        {
            IQueryable<ProductModel> products = _productRepository.GetProductsAsync().Result;
            return View();
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
