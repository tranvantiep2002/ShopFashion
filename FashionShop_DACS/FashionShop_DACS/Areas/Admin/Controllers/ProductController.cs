using FashionShop_DACS.Helper.Abstract;
using FashionShop_DACS.Models;
using FashionShop_DACS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace FashionShop_DACS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpClientService _httpClientService;

        private const string baseUrlProduct = "http://localhost:7091/api/Product";
        private const string baseUrlCategory = "http://localhost:7091/api/Category";

        public ProductController(IProductRepository productRepository, 
                                 ICategoryRepository categoryRepository,
                                 IHttpClientFactory httpClientFactory,
                                 IHttpClientService httpClientService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _httpClientFactory = httpClientFactory;
            _httpClientService = httpClientService;
        }

        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var result = await _httpClientService.GetDataAsync(baseUrlProduct);

            if (string.IsNullOrEmpty(result))
            {
                return View(new List<Product>());
            }

            var products = JsonConvert.DeserializeObject<List<Product>>(result);
            return View(products);
        }

        // Hiển thị form thêm sản phẩm mới
        public async Task<IActionResult> Add()
        {
            var result = await _httpClientService.GetDataAsync(baseUrlCategory);

            ViewBag.Categories = new SelectList(string.IsNullOrEmpty(result) 
                                        ? new List<Category>() 
                                        : JsonConvert.DeserializeObject<List<Category>>(result), "Id", "Name");
            return View();
        }

        // Xử lý thêm sản phẩm mới
        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    // Lưu hình ảnh đại diện tham khảo bài 02 hàm SaveImage
                    product.ImageUrl = await SaveImage(imageUrl);
                }

                var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                await _httpClientService.PostDataAsync(baseUrlProduct, content);

                return RedirectToAction(nameof(Index));
            }
            // Nếu ModelState không hợp lệ, hiển thị form với dữ liệu đã nhập
            var categories = await _httpClientService.GetDataAsync(baseUrlCategory);

            ViewBag.Categories = new SelectList(string.IsNullOrEmpty(categories) 
                    ? new List<Category>() 
                    : JsonConvert.DeserializeObject<List<Category>>(categories), "Id", "Name");

            return View(product);
        }

        private async Task<string> SaveImage(IFormFile imageUrl)
        {
            var savePath = Path.Combine("wwwroot/images", imageUrl.FileName); // Thay đổi đường dẫn theo cấu hình của bạn
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await imageUrl.CopyToAsync(fileStream);
            }
            return "/images/" + imageUrl.FileName; // Trả về đường dẫn tương đối

        }

        // Hiển thị form cập nhật sản phẩm
        public async Task<IActionResult> Update(int id)
        {
            var result = await _httpClientService.GetDataAsync($"{baseUrlProduct}/{id}");

            if (string.IsNullOrEmpty(result))
            {
                return NotFound();
            }

            var product = JsonConvert.DeserializeObject<Product>(result);

            var categories = await _httpClientService.GetDataAsync(baseUrlCategory);

            ViewBag.Categories = new SelectList(string.IsNullOrEmpty(categories) 
                        ? new List<Category>() 
                        : JsonConvert.DeserializeObject<List<Category>>(categories), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // Xử lý cập nhật sản phẩm
        [HttpPost]
        public async Task<IActionResult> Update(int id, Product product, IFormFile imageUrl)
        {
            ModelState.Remove("ImageUrl"); // Loại bỏ xác thực ModelState cho ImageUrl
            if (id != product.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var result = await _httpClientService.GetDataAsync($"{baseUrlProduct}/{id}"); // Giả định có phương thức GetByIdAsync

                var existingProduct = JsonConvert.DeserializeObject<Product>(result);

                if (imageUrl == null)
                {
                    product.ImageUrl = existingProduct.ImageUrl;
                }
                else
                {
                    // Lưu hình ảnh mới
                    product.ImageUrl = await SaveImage(imageUrl);
                }
                // Cập nhật các thông tin khác của sản phẩm
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.ImageUrl = product.ImageUrl;
                existingProduct.Category = JsonConvert.DeserializeObject<Category>(await _httpClientService.GetDataAsync($"{baseUrlCategory}/{product.CategoryId}"));

                var content = new StringContent(JsonConvert.SerializeObject(existingProduct), Encoding.UTF8, "application/json");
                await _httpClientService.PutDataAsync(baseUrlProduct, content);

                return RedirectToAction(nameof(Index));
            }

            var categories = await _httpClientService.GetDataAsync(baseUrlCategory);

            ViewBag.Categories = new SelectList(string.IsNullOrEmpty(categories)
                        ? new List<Category>()
                        : JsonConvert.DeserializeObject<List<Category>>(categories), "Id", "Name");
            return View(product);
        }



        // Hiển thị thông tin chi tiết sản phẩm
        public async Task<IActionResult> Display(int id)
        {
            var result = await _httpClientService.GetDataAsync($"{baseUrlProduct}/{id}");

            if (string.IsNullOrEmpty(result))
            {
                return NotFound();
            }

            var product = JsonConvert.DeserializeObject<Product>(result);

            return View(product);
        }
        // Hiển thị form xác nhận xóa sản phẩm
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _httpClientService.GetDataAsync($"{baseUrlProduct}/{id}");

            if (string.IsNullOrEmpty(result))
            {
                return NotFound();
            }

            var product = JsonConvert.DeserializeObject<Product>(result);

            return View(product);
        }

        // Xử lý xóa sản phẩm
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _httpClientService.DeleteDataAsync($"{baseUrlProduct}?id={id}");

            return RedirectToAction(nameof(Index));
        }
    }
}
