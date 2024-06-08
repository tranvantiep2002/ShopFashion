using FashionShop_DACS.Helper.Abstract;
using FashionShop_DACS.Models;
using FashionShop_DACS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;


namespace FashionShop_DACS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHttpClientService _httpClientService;
        private const string baseUrlCategory = "http://localhost:7091/api/Category";

        public CategoryController(IProductRepository productRepository, ICategoryRepository categoryRepository, IHttpClientService httpClientService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _httpClientService = httpClientService;
        }

        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var result = await _httpClientService.GetDataAsync(baseUrlCategory);

            if (string.IsNullOrEmpty(result))
            {
                return View(new List<Category>());
            }

            var categories = JsonConvert.DeserializeObject<List<Category>>(result);
            return View(categories);
        }
        // Hiển thị form thêm sản phẩm mới
        public async Task<IActionResult> Add()
        {
            return View();
        }

        // Xử lý thêm sản phẩm mới
        [HttpPost]
        public async Task<IActionResult> Add(Category category)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");

                await _httpClientService.PostDataAsync("http://localhost:7091/api/Category", content);
                
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // Hiển thị thông tin chi tiết sản phẩm
        public async Task<IActionResult> Display(int id)
        {
            var result = await _httpClientService.GetDataAsync($"{baseUrlCategory}/{id}");

            if (string.IsNullOrEmpty(result))
            {
                return NotFound();
            }

            var category = JsonConvert.DeserializeObject<Category>(result);
            return View(category);
        }

        // Hiển thị form cập nhật sản phẩm
        public async Task<IActionResult> Update(int id)
        {
            var result = await _httpClientService.GetDataAsync($"{baseUrlCategory}/{id}");

            if (string.IsNullOrEmpty(result))
            {
                return NotFound();
            }

            var category = JsonConvert.DeserializeObject<Category>(result);
            return View(category);
        }
        // Xử lý cập nhật sản phẩm
        [HttpPost]
        public async Task<IActionResult> Update(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
                await _httpClientService.PutDataAsync(baseUrlCategory, content);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        // Hiển thị form xác nhận xóa sản phẩm
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _httpClientService.GetDataAsync($"{baseUrlCategory}/{id}");

            if (string.IsNullOrEmpty(result))
            {
                return NotFound();
            }

            var category = JsonConvert.DeserializeObject<Category>(result);
            return View(category);
        }

        // Xử lý xóa sản phẩm
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _httpClientService.GetDataAsync($"{baseUrlCategory}/{id}");

            if (category != null)
            {
                await _httpClientService.DeleteDataAsync($"{baseUrlCategory}?id={id}");
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Test1()
        {
            return View();
        }
    }
}
