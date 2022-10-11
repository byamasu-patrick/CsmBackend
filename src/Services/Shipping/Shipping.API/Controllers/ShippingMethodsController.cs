using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Payment.API.Models;
using Shipping.API.Entities;
using Shipping.API.Models;
using Shipping.API.Repositories.Interfaces;
using System.Net;


namespace Shipping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingMethodsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IShippingMethodRepository _repository;
        private readonly ILogger<ShippingMethodsController> _logger;
        private IMapper _mapper;

        public ShippingMethodsController(IConfiguration configuration, IShippingMethodRepository repository,
            ILogger<ShippingMethodsController> logger, IMapper mapper)
        {
            _configuration = configuration;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{page}")]
        [ProducesResponseType(typeof(ShippingResponse<ShippingMethods>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShippingResponse<ShippingMethods>>> GetShippingMethods(int page)
        {
            var shippings = await _repository.GetShippingMethods(page);
            return Ok(shippings);
        }

        [HttpGet("{id:length(24)}", Name = "GetShippingMethodsById")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ShippingMethods), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShippingMethods>> GetShippingMethodsById(string id)
        {
            var payment = await _repository.GetShippingMethod(id);

            if (payment == null)
            {
                _logger.LogError($"Shipping Method with id: {id}, not found.");
                return NotFound();
            }

            return Ok(payment);
        }

        [Route("[action]/{userId}", Name = "GetShippingMethodsUserId")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ShippingMethods>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ShippingMethods>>> GetShippingMethodsUserId(string userId)
        {
            var payments = await _repository.GetShippingMethodUserId(userId);
            return Ok(payments);
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteShippingMethodsById")]
        [ProducesResponseType(typeof(ShippingAddress), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteShippingMethodsById(string id)
        {
            return Ok(await _repository.DeleteShippingMethod(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShippingMethods), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShippingMethods>> CreateProduct([FromBody] CreateShippingMethodDto shippingMethodDto)
        {
            var shippingMethod = _mapper.Map<ShippingMethods>(shippingMethodDto);

            shippingMethod.Id = ObjectId.GenerateNewId().ToString();
            shippingMethod.CreatedAt = DateTime.UtcNow;

            await _repository.CreateShippingMethod(shippingMethod);

            return Ok(shippingMethod);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ShippingMethods), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] ShippingMethods shipping)
        {
            return Ok(await _repository.UpdateShippingMethod(shipping));
        }

    }
}
