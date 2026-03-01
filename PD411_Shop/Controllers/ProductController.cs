using Microsoft.AspNetCore.Mvc;
using PD411_Shop.Data;
using PD411_Shop.Models;
using PD411_Shop.ViewModels;

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
            Console.WriteLine("get");
            return View(products);
        }
        //get
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateProductVM vm) 
        {
            
            var category = _context.Categories.FirstOrDefault();
            if (category == null)
            {
                return View();
            }
            ProductModel model = new ProductModel
            {
                Name = vm.Name ?? string.Empty,
                Amount = vm.Amount,
                Color = vm.Color,
                Description = vm.Description,
                Price = vm.Price,
                Category = category,

            };
            if (vm.Image != null) 
            {
                string root = Directory.GetCurrentDirectory();
                string imagePath = Path.Combine(root, "wwwroot", "images");
                string ext = Path.GetExtension(vm.Image.FileName);
                string name = Guid.NewGuid().ToString();
                string fileName = name + ext;
                string filePath = Path.Combine(imagePath, fileName);
                using var imageStream = vm.Image.OpenReadStream();
                using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                imageStream.CopyTo(fileStream);
                model.Image = fileName;
            }
            model.Category = category;
            await _context.Products.AddAsync(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if( product != null)
            {
                if(product.Image != null)
                {
                    string root = Directory.GetCurrentDirectory();
                    string imagesPath = Path.Combine(root, "wwwroot", "images");
                    string filePath = Path.Combine($"{imagesPath}", product.Image);
                    if (System.IO.File.Exists(imagesPath))
                    {
                        System.IO.File.Delete(imagesPath);
                    }
                    
                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("index");
        }
    }
}
