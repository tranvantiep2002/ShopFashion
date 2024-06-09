using FashionShop_DACS.Areas.Identity.Pages.Account;
using FashionShop_DACS.Helper.Abstract;
using FashionShop_DACS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using NuGet.Common;
using System.Text;
using static FashionShop_DACS.Areas.Identity.Pages.Account.LoginModel;

namespace FashionShop_DACS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpClientService _httpClientService;
        private readonly FashionShop_DACS.Helper.Abstract.IEmailSender _emailSender;
        private const string baseUrlUser = "http://localhost:7091/api/auth";

        public UserController(UserManager<ApplicationUser> userManager, 
                SignInManager<ApplicationUser> signInManager,
                IHttpClientService httpClientService,
                IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpClientService = httpClientService;
            _emailSender = emailSender;
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
                var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var content = new StringContent(JsonConvert.SerializeObject(new
                    {
                        loginModel.Email,
                        loginModel.Password,
                    }), Encoding.UTF8, "application/json");

                    var tokenJson = await _httpClientService.PostDataAsync($"{baseUrlUser}/login", content);

                    if (!string.IsNullOrEmpty(tokenJson))
                    {
                        CookieOptions option = new CookieOptions();
                        option.Expires = DateTime.Now.AddHours(24);

                        var token = JsonConvert.DeserializeObject<TokenModel>(tokenJson);
                        Response.Cookies.Append("Token", token.Token, option);
                    }


                    return Redirect("~/");
                }
                // Nếu đăng nhập thất bại, hiển thị thông báo lỗi
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            // Nếu có lỗi, hiển thị lại form đăng nhập với thông báo lỗi
            return View(loginModel);
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if(user is not null)
                {
                    var hasPassword = await _userManager.HasPasswordAsync(user);
                    IdentityResult result;

                    if (hasPassword)
                    {
                        result = await _userManager.RemovePasswordAsync(user);

                        if (result.Succeeded)
                        {
                            result = await _userManager.AddPasswordAsync(user, "Test@1234678");
                            await _emailSender.SendEmailAsync(model.Email, "Reset password", "Password: Test@1234678");
                        }
                    }
                }
            }

            return Redirect("/Admin/User/Login");
        }

        public IActionResult LockAccountTest()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LockAccountTest(LockAccountModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is not null)
                {
                    user.LockoutEnd = DateTimeOffset.MaxValue;

                    var result = await _userManager.UpdateAsync(user);
                }
            }
            return Redirect("/~");
        }
    }
}
