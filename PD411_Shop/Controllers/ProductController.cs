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
               
                model.Image = await _imageService.SaveImageAsync(vm.Image, PathSettings.Products);
            }
 
            await _productRepository.CreateAsync(model);

            return RedirectToAction("index");
        }
        // ------------------------get-------------------
        public async Task<IActionResult> Update(int id)
        {
            var model = await _productRepository.GetByIdAsync(id);
            if (model == null)
            {
                RedirectToAction("index");
            }
            var viewModel = new UpdateProductVM
            {
                Id = model.Id,
                Amount = model.Amount,
                CategoryId = model.CategoryId,
                Color = model.Color,
                Description = model.Description,
                Price = model.Price,
                Name = model.Name,
                SelectCategories = await GetSelectCategoriesAsync()


            };
            return View(viewModel);
        }
        // ---------------------update-------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateProductVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.SelectCategories = await GetSelectCategoriesAsync();
                return View(viewModel);
            }
            var model = await _productRepository.GetByIdAsync(viewModel.Id);
            if (model != null)
            {
                model.Name = viewModel.Name!;
                model.Description = viewModel.Description!;
                model.Price = viewModel.Price!;
                model.Color = viewModel.Color!;
                model.Amount = viewModel.Amount!;
                model.CategoryId = viewModel.CategoryId!;
                if( viewModel.Image != null)
                {
                    string? ImageName = await _imageService.SaveImageAsync(viewModel.Image, "products");
                    if (ImageName != null)
                    {
                        if (!string.IsNullOrEmpty(model.Image))
                        {
                            _imageService.DeleteImage("products", model.Image);
                        }
                    }
                    model.Image = ImageName;
                }
               await _productRepository.UpdateAsync(model);
            }
            return RedirectToAction("index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            
            var product = await _productRepository.GetByIdAsync(id);
            if( product != null)
            {
                if(product.Image != null)
                {
                    
                    _imageService.DeleteImage(PathSettings.Products, product.Image);
                    
                }
              
                await _productRepository.DeleteAsync(product.Id);
            }
            return RedirectToAction("index");
        }
    }
}
