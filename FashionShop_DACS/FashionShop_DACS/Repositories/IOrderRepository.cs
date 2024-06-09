using FashionShop_DACS.Models;

namespace FashionShop_DACS.Repositories;

public interface IOrderRepository
{
    Task<List<Order>> GetListAsync();

    Task<List<Order>> GetListByUserAsync(string id);
}
