using PD411_Shop.Data;

namespace PD411_Shop.Repositories
{
    
    public class GenericRepository<TModel>
        where TModel: class, IBaseModel
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(TModel model)
        {
           await  _context.Set<TModel>().AddAsync(model);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(TModel model)
        {
            await _context.Set<TModel>().AddAsync(model);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var model = await GetByIdAsync(id);
            if (model != null)
            {
                _context.Set<TModel>().Remove(model);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<TModel?> GetByIdAsync(int id)
        {
            return await _context.Set<TModel>().FindAsync(id);
        }
    }
}
