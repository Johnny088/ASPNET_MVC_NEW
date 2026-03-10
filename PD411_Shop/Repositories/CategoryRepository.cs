using Microsoft.EntityFrameworkCore;
using PD411_Shop.Data;
using PD411_Shop.Models;

namespace PD411_Shop.Repositories
    
{
    public class CategoryRepository
        :GenericRepository<CategoryModel>
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }
        public async Task<List<CategoryModel>> GetAllAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
