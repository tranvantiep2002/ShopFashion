using Microsoft.AspNetCore.Mvc;

namespace FashionShop_DACS.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
