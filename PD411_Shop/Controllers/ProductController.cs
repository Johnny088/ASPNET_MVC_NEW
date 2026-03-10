using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PD411_Shop.Data;
using PD411_Shop.Models;
using PD411_Shop.Repositories;
using PD411_Shop.Services;
using PD411_Shop.Settings;
using PD411_Shop.ViewModels;

namespace PD411_Shop.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductController : Controller
    {
        
        private readonly ProductRepository _productRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly ImageService _imageService;
        public ProductController( ProductRepository productRepository, CategoryRepository categoryRepository, ImageService imageService)
        {
            
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _imageService = imageService;
        }
        private async Task<IEnumerable<SelectListItem>> GetSelectCategoriesAsync()
        {
            //List<CategoryModel> categories = await _context.Categories.ToListAsync(); //-----------------------temp------------------------
            List<CategoryModel> categories = await _categoryRepository.GetAllAsync();
            IEnumerable<SelectListItem> selectItems = categories
                .Select(c => new SelectListItem(c.Name, c.Id.ToString()));
            return selectItems;
        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "identity" });
            }

            //var products = _context.Products.Include(p => p.Category).AsNoTracking().AsEnumerable();
            var products = await _productRepository.GetAllAsync(new PaginationVM { PageSize = 200 });   
            return View(products);
        }
        //get
        public async Task<IActionResult> Create()
        {

            var viewModel = new CreateProductVM
            {
                SelectCategories = await GetSelectCategoriesAsync()
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // defends from xcrf attaks  (fake account with someone's token)
        public async Task<IActionResult> Create([FromForm]CreateProductVM vm) 
        {
           
            if (!ModelState.IsValid)
            {
                vm.SelectCategories = await GetSelectCategoriesAsync();
                return View(vm);
            }

            //var category = _context.Categories.FirstOrDefault();

            //if (category == null)
            //{
            //    return View();
            //}
            ProductModel model = new ProductModel
            {
                Name = vm.Name ?? string.Empty,
                Amount = vm.Amount,
                Color = vm.Color,
                Description = vm.Description,
                Price = vm.Price,
                CategoryId = vm.CategoryId,

            };
            if (vm.Image != null) 
            {
                //string root = Directory.GetCurrentDirectory();
                //string imagePath = Path.Combine(root, "wwwroot", "images");
                //string ext = Path.GetExtension(vm.Image.FileName);
                //string name = Guid.NewGuid().ToString();
                //string fileName = name + ext;
                //string filePath = Path.Combine(imagePath, fileName);
                //using var imageStream = vm.Image.OpenReadStream();
                //using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                //imageStream.CopyTo(fileStream);
                //model.Image = fileName;
                model.Image = await _imageService.SaveImageAsync(vm.Image, PathSettings.Products);
            }
            //model.Category = category;
            //await _context.Products.AddAsync(model);
            //await _context.SaveChangesAsync();
            await _productRepository.CreateAsync(model);

            return RedirectToAction("index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            //var product = _context.Products.FirstOrDefault(p => p.Id == id);
            var product = await _productRepository.GetByIdAsync(id);
            if( product != null)
            {
                if(product.Image != null)
                {
                    //string root = Directory.GetCurrentDirectory();
                    //string imagesPath = Path.Combine(root, "wwwroot", "images");
                    //string filePath = Path.Combine($"{imagesPath}", product.Image);
                    //if (System.IO.File.Exists(imagesPath))
                    //{
                    //    System.IO.File.Delete(filePath);
                    //}
                    _imageService.DeleteImage(PathSettings.Products, product.Image);
                    
                }
                //_context.Products.Remove(product);
                //await _context.SaveChangesAsync();
                await _productRepository.DeleteAsync(product.Id);
            }
            return RedirectToAction("index");
        }
    }
}
