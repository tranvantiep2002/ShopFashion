using FashionShop_DACS.Models;
using FashionShop_DACS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FashionShop_DACS.Areas.Admin.Controllers;

[Area("Admin")]
public class OrderController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    public OrderController(IOrderRepository orderRepository, UserManager<ApplicationUser> userManager)
    {
        _orderRepository = orderRepository;
        _userManager = userManager;
    }
    // GET: OrderController
    [Authorize(Roles = SD.Role_Admin)]
    public async Task<IActionResult> Index()
    {
        var result = await _orderRepository.GetListAsync();
        return View(result is null ? new List<Order>() : result);
    }

    // GET: OrderController/Details/5
    public async Task<IActionResult> ListOrder()
    {
        var user = await _userManager.GetUserAsync(User);

        var result = await _orderRepository.GetListByUserAsync(user.Id);

        return View(result is null ? new List<Order>() : result);
    }

    // GET: OrderController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: OrderController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: OrderController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: OrderController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: OrderController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: OrderController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}
