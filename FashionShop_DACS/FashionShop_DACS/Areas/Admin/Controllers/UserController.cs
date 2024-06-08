using FashionShop_DACS.Areas.Identity.Pages.Account;
using FashionShop_DACS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static FashionShop_DACS.Areas.Identity.Pages.Account.LoginModel;

namespace FashionShop_DACS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(InputModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginModel.Email);
                if (user != null)
                {
                    if (await _userManager.CheckPasswordAsync(user, loginModel.Password))
                    {
                        // Đăng nhập thành công, chuyển hướng đến trang Home
                        return Redirect("~/");
                    }
                }
                // Nếu đăng nhập thất bại, hiển thị thông báo lỗi
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            // Nếu có lỗi, hiển thị lại form đăng nhập với thông báo lỗi
            return View(loginModel);
        }
    }
}
