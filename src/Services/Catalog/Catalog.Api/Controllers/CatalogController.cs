using Catalog.Api.Entitites;
using Catalog.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Net;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;
        public CatalogController(ILogger<CatalogController> logger, IProductRepository productRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepository.GetProducts();
            return Ok(products);
        }
        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _productRepository.GetProduct(id);
            if (product == null)
            {
                return StatusCode(404);
            }
            return Ok(product);
        }
        [HttpGet("{productName:length(15)}")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Product>> GetProductByName(string productName)
        {
            var product = await _productRepository.GetProductByName(productName);
            if (product == null)
            {
                return StatusCode(404);
            }
            return Ok(product);
        }
        [HttpGet("{categoryName:length(15)}")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Product>> GetProductByCategory(string categoryName)
        {
            var product = await _productRepository.GetProductByCategory(categoryName);
            if (product == null)
            {
                return StatusCode(404);
            }
            return Ok(product);
        }
        [HttpDelete("{id:length(15)}", Name="DeleteProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<bool>> DeleteProductbyId(string id){
            var isdeleted = await _productRepository.DeleteProduct(id);
            if (!isdeleted)
            {
                return StatusCode(404);
            }
            return Ok(isdeleted);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateProduct(Product product){
            if (product == null)
            {
                return StatusCode(400);
            }
            await _productRepository.CreateProduct(product);
            return StatusCode(201);
        }
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> UpdateProduct(Product product){
            if (product == null)
            {
                return StatusCode(400);
            }
            await _productRepository.UpdateProduct(product);
            return StatusCode(204);
        }
    }
}
