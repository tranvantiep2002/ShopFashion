using FashionShop_DACS.Models;
using FashionShop_DACS.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FashionShop_DACS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async ValueTask<List<Product>> GetListAsync()
        {
            return (await _productRepository.GetAllAsync()).ToList();
        }

        [HttpPost]
        public async ValueTask CreateAsync(Product product)
        {
            await _productRepository.AddAsync(product);
        }

        [HttpGet("{id}")]
        public async ValueTask<Product> GetAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        [HttpPut]
        public async ValueTask UpdateAsync(Product product)
        {
            await _productRepository.UpdateAsync(product);
        }

        [HttpDelete]
        public async ValueTask DeleteAsync(int id)
        {
            await _productRepository.DeleteAsync(id);
        }

    }
}
