using Microsoft.EntityFrameworkCore;
using PD411_Shop.Data;
using PD411_Shop.Models;

namespace PD411_Shop.Repositories
{
    public class ProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IQueryable<ProductModel>> GetProductsAsync()
        {
            return _context.Products.AsNoTracking();
        }

    }
}




