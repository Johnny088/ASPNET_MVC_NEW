using PD411_Shop.Repositories;

namespace PD411_Shop.Models
{
    public class CategoryModel: BaseModel
    {
        public required string Name { get; set; }
        public string Icon { get; set; } = "bi-activity";

        public List<ProductModel> Products { get; set; } = [];
    }
}
