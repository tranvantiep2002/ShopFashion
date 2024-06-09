using FashionShop_DACS.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace FashionShop_DACS.Repositories;

public class EFOrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;
    public EFOrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetListAsync()
    {
        return await _context.Orders.Include(x => x.ApplicationUser).ToListAsync();
    }

    public async Task<List<Order>> GetListByUserAsync(string id)
    {
        return await _context.Orders.Include(x => x.ApplicationUser).Where(x => x.UserId == id).ToListAsync();
    }
}
