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
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ShippingAddressController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private static string s_wasmClientURL;
        private readonly IShippingAddressRepository _repository;
        private readonly ILogger<ShippingAddressController> _logger;
        private IMapper _mapper;

        public ShippingAddressController(IConfiguration configuration, IShippingAddressRepository repository,
            ILogger<ShippingAddressController> logger, IMapper mapper)
        {
            _configuration = configuration;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{page}")]
        [ProducesResponseType(typeof(ShippingResponse<ShippingAddress>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShippingResponse<ShippingAddress>>> ShippingAddresses(int page)
        {
            var shippings = await _repository.GetShippingAddresses(page);
            return Ok(shippings);
        }

        [HttpGet("{id:length(24)}", Name = "GetShippingAddressById")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ShippingAddress), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShippingAddress>> GetShippingAddressById(string id)
        {
            var payment = await _repository.GetShippingAddress(id);

            if (payment == null)
            {
                _logger.LogError($"Shipping Address with id: {id}, not found.");
                return NotFound();
            }

            return Ok(payment);
        }

        [Route("[action]/{userId}", Name = "GetShippingAddressUserId")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ShippingAddress>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ShippingAddress>>> GetShippingAddressUserId(string userId)
        {
            var payments = await _repository.GetShippingAddressUserId(userId);
            return Ok(payments);
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteShippingAddressById")]
        [ProducesResponseType(typeof(ShippingAddress), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteShippingAddressById(string id)
        {
            return Ok(await _repository.DeleteShippingAddress(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShippingAddress), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShippingAddress>> CreateProduct([FromBody] CreateShippingAddresseDto shippingAddressDto)
        {
            var shippingAddress = _mapper.Map<ShippingAddress>(shippingAddressDto);

            shippingAddress.Id = ObjectId.GenerateNewId().ToString();
            shippingAddress.CreatedAt = DateTime.UtcNow;

            await _repository.CreateShippingAddress(shippingAddress);

            return Ok(shippingAddress);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ShippingAddress), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] ShippingAddress shippingAddress)
        {
            return Ok(await _repository.UpdateShippingAddress(shippingAddress));
        }

    }
}
