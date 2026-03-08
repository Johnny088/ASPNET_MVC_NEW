using PD411_Shop.Models;

namespace PD411_Shop.ViewModels
{
    public class CartItemVM
    {
        public int ProductId { get; set; }
        public int Count { get; set; } = 1;
        public double TotalPrice => ProductId * Count;
    }
}
