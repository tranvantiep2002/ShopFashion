using FashionShop_DACS.Models;
using FashionShop_DACS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FashionShop_DACS.Areas.EmployerControllers
{
	[Area("Employer")]
	[Authorize(Roles = SD.Role_Employer)]
	public class ProductController : Controller
	{
		private readonly IProductRepository _productRepository;
		private readonly ICategoryRepository _categoryRepository;

		public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
		{
			_productRepository = productRepository;
			_categoryRepository = categoryRepository;
		}

		// Hiển thị danh sách sản phẩm
		public async Task<IActionResult> Index()
		{
			var products = await _productRepository.GetAllAsync();
			return View(products);
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
			var product = await _productRepository.GetByIdAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			var categories = await _categoryRepository.GetAllAsync();
			ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
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
				var existingProduct = await _productRepository.GetByIdAsync(id); // Giả định có phương thức GetByIdAsync
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
				await _productRepository.UpdateAsync(existingProduct);
				return RedirectToAction(nameof(Index));
			}
			var categories = await _categoryRepository.GetAllAsync();
			ViewBag.Categories = new SelectList(categories, "Id", "Name");
			return View(product);
		}

		// Hiển thị thông tin chi tiết sản phẩm
		public async Task<IActionResult> Display(int id)
		{
			var product = await _productRepository.GetByIdAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			return View(product);
		}
		
	}
}
