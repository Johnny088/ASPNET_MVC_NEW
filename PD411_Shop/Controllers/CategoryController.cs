using Microsoft.AspNetCore.Mvc;
using PD411_Shop.Models;
using PD411_Shop.Repositories;
using PD411_Shop.ViewModels;

namespace PD411_Shop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryController(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryVM viewModel)
        {
            var model = new CategoryModel
            {
                Name = viewModel.Name ?? String.Empty
            };
            await _categoryRepository.CreateAsync(model);
            return RedirectToAction("Index", "Home");
        }
    }
}
