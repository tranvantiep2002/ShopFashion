using FashionShop_DACS.Models;

namespace FashionShop_DACS.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
        object OrderByDescending(Func<object, object> value);
    }

}
