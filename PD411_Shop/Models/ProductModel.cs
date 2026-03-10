using PD411_Shop.Repositories;

namespace PD411_Shop.Models
{
    public class ProductModel: BaseModel
    {
        //public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public string? Image { get; set; }
        public int Amount { get; set; }
        public string? Color { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int CategoryId { get; set; }
        public CategoryModel? Category { get; set; }
    }
}
