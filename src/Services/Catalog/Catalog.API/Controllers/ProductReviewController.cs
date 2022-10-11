using AutoMapper;
using Catalog.API.Entities;
using Catalog.API.Models;
using Catalog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductReviewController : ControllerBase
    {
        private readonly IProductReviewRepository _repository;
        private readonly ILogger<ProductReviewController> _logger;
        private IMapper _mapper;

        public ProductReviewController(IProductReviewRepository repository, ILogger<ProductReviewController> logger, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Route("[action]/{productId}/{page}", Name = "GetReviewByproductId")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductReview>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ProductReview>>> GetReviewByproductId(string productId, int page)
        {
            var products = await _repository.GetProductReviewByProductId(productId, page);
            return Ok(products);
        }

        [Route("[action]/{reviewId}", Name = "GetReviewById")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetReviewById(string reviewId)
        {
            var items = await _repository.GetReview(reviewId);
            if (items == null)
            {
                _logger.LogError($"Products Rewiew with id: {reviewId} not found.");
                return NotFound();
            }
            return Ok(items);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductReview), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductReview>> CreateProduct([FromBody] CreateReviewDto reviewDto)
        {
            var productReview = _mapper.Map<ProductReview>(reviewDto);

            productReview.Id = ObjectId.GenerateNewId().ToString();
            productReview.CreatedAt = DateTime.UtcNow;

            await _repository.CreateReview(productReview);

            return Ok(productReview);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ProductReview), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateReview([FromBody] ProductReview productReview)
        {
            return Ok(await _repository.UpdateReview(productReview));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProductReview")]
        [ProducesResponseType(typeof(ProductReview), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductReview(string id)
        {
            return Ok(await _repository.DeleteReview(id));
        }


    }
}
