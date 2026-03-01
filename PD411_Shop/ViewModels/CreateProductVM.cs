using Microsoft.AspNetCore.Mvc.Rendering;
using PD411_Shop.Models;
using System.ComponentModel.DataAnnotations;

namespace PD411_Shop.ViewModels
{
    public class CreateProductVM
    {
        [Required(ErrorMessage = "Required field"), MaxLength(200, ErrorMessage = "Max length is 200 symbols ")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Range(0d, 9999999d, ErrorMessage = "The Range has to be between 0 - 9999999")]
        public double Price { get; set; } = 0d;
        public IFormFile? Image { get; set; }
        [Range(0d, 1000, ErrorMessage = "The Range has to be between 0 - 1000")]
        public int Amount { get; set; } = 0;
        [Required(ErrorMessage = "Required field"), MaxLength(100, ErrorMessage = "Max length is 200 symbols ")]
        public string? Color { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<SelectListItem> SelectCategories { get; set; } = [];

    }
}
