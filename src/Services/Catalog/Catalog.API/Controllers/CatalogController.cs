using AutoMapper;
using Catalog.API.Entities;
using Catalog.API.Models;
using Catalog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Nest;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;
        private readonly IElasticClient _elasticClient;
        private IMapper _mapper;

        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger, IMapper mapper, IElasticClient elasticClient)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
        }

        [HttpGet("{page}")]
        [ProducesResponseType(typeof(ProductResponse<Product>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<ProductResponse<Product>>> GetProducts(int page)
        {
            var products = await _repository.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetProductById(string id)
        {
            var product = await _repository.GetProduct(id);

            if (product == null)
            {
                _logger.LogError($"Product with id: {id}, not found.");
                return NotFound();
            }

            return Ok(product);
        }

        [Route("[action]/{category}/{page}", Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(ProductResponse<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category, int page)
        {
            var products = await _repository.GetProductByCategory(category, page);
            return Ok(products);
        }
        [Route("[action]/{keyword}/{page}", Name = "SearchProducts")]
        [HttpGet]
        [ProducesResponseType(typeof(ProductResponse<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> SearchPrducts(string keyword, int page = 1)
        {
            var products = await _repository.SearchProducts(keyword, page);
            return Ok(products);
        }

        [Route("[action]/{ownerId}", Name = "GetProductByOwner")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByOwner(string ownerId)
        {
            var products = await _repository.GetProductByShopOwner(ownerId);
            return Ok(products);
        }

     /*   [Route("[action]/{keyword}", Name = "SearchProducts")]
        [HttpGet]
        public async Task<IActionResult> SearchProducts(string keyword)
        {
         

            var result = await _elasticClient.SearchAsync<Product>(
                s => s.Query(
                    q => q.QueryString(
                        d => d.Query('*' + keyword + '*')
                        )
                    ).Size(1000)
                );


            return Ok(result.Documents.ToList());

        } */

        [Route("[action]/{name}", Name = "GetProductByName")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByName(string name)
        {
            var items = await _repository.GetProductByName(name);
            if (items == null)
            {
                _logger.LogError($"Products with name: {name} not found.");
                return NotFound();
            }
            return Ok(items);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            product.Id = ObjectId.GenerateNewId().ToString();
            product.CreatedAt = DateTime.UtcNow;

            await _repository.CreateProduct(product);
            await _elasticClient.IndexDocumentAsync(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await _repository.UpdateProduct(product));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            return Ok(await _repository.DeleteProduct(id));
        }

    }
}
