using Microsoft.EntityFrameworkCore;
using PD411_Shop.Data;
using PD411_Shop.Models;
using PD411_Shop.ViewModels;

namespace PD411_Shop.Repositories
{
    public class ProductRepository: GenericRepository<ProductModel>
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        //public async Task<ProductModel?> GetByIdAsync(int id)
        //{
        //    var res = await _context.Products.FindAsync(id);
        //    return res;
        //}
        //public async Task CreateAsync(ProductModel model) 
        //{
        //    await _context.Products.AddAsync(model);
        //    await _context.SaveChangesAsync();
        //}
        //public async Task UpdateAsync(ProductModel model) 
        //{
        //    _context.Products.Update(model);
        //    await _context.SaveChangesAsync();
        //}
        //public async Task DeleteAsync(int id) 
        //{
        //    var model = await GetByIdAsync(id);
        //    if (model != null)
        //    {
        //        _context.Products.Remove(model);
        //        await _context.SaveChangesAsync();
        //    }
        //}
        public async Task<List<ProductModel>> GetAllAsync(PaginationVM pagination, int? category = null)
        {
            IQueryable<ProductModel> products = 
                _context.Products
                .AsNoTracking()
                .Include(p => p.Category);
            if (category != null && _context.Categories.Any(c => c.Id == category))
            {
                products = products.Where(p => p.CategoryId == category);
            }
            
            // Pagination
            pagination.PageSize = pagination.PageSize < 1 ? 20 : pagination.PageSize;
            pagination.PageCount = (int)Math.Ceiling((double)products.Count() / pagination.PageSize);
            pagination.Page = pagination.Page < 1 || pagination.Page > pagination.PageCount ? 1 : pagination.Page;

            products = products
                .Skip(pagination.PageSize * (pagination.Page - 1))
                .Take(pagination.PageSize)
                .OrderBy(p => p.Id);
            return await products.ToListAsync();
        }

    }
}




