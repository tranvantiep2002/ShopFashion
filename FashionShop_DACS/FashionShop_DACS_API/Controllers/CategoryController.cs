using FashionShop_DACS.Models;
using FashionShop_DACS.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FashionShop_DACS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async ValueTask<List<Category>> GetListAsync()
        {
            return (await _categoryRepository.GetAllAsync()).ToList();            
        }

        [HttpPost]
        public async ValueTask CreateAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);
        }

        [HttpGet("{id}")]
        public async ValueTask<Category> DetailAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        [HttpPut]
        public async ValueTask UpdateAsync(Category category)
        {
            await _categoryRepository.UpdateAsync(category);
        }

        [HttpDelete]
        public async ValueTask DeleteAsync(int id)
        {
            await _categoryRepository.DeleteAsync(id);
        }
    }
}
