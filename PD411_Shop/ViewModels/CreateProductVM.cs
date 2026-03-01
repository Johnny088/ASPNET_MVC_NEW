using PD411_Shop.Models;

namespace PD411_Shop.ViewModels
{
    public class CreateProductVM
    {
        public required string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public IFormFile? Image { get; set; }
        public int Amount { get; set; }
        public string? Color { get; set; }


    }
}
