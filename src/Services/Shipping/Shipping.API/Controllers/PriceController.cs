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
    public class PriceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private static string s_wasmClientURL;
        private readonly IPriceRepository _repository;
        private readonly ILogger<PriceController> _logger;
        private IMapper _mapper;

        public PriceController(IConfiguration configuration, IPriceRepository repository,
            ILogger<PriceController> logger, IMapper mapper)
        {
            _configuration = configuration;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{page}")]
        [ProducesResponseType(typeof(ShippingResponse<Prices>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShippingResponse<Prices>>> Prices(int page)
        {
            var prices = await _repository.GetPrices(page);
            return Ok(prices);
        }

        [HttpGet("{id:length(24)}", Name = "GetPriceById")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Prices), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Prices>> GetPriceById(string id)
        {
            var price = await _repository.GetPrice(id);

            if (price == null)
            {
                _logger.LogError($"Price  with id: {id}, not found.");
                return NotFound();
            }

            return Ok(price);
        }

        [HttpDelete("{id:length(24)}", Name = "DeletePriceById")]
        [ProducesResponseType(typeof(Prices), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeletePriceBy(string id)
        {
            return Ok(await _repository.DeletePrice(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Courier), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Prices>> CreatePrice([FromBody] CreatePriceDto priceDto)
        {
            var price = _mapper.Map<Prices>(priceDto);

            price.Id = ObjectId.GenerateNewId().ToString();

            await _repository.CreatePrice(price);

            return Ok(price);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Prices), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdatePrice([FromBody] Prices price)
        {
            return Ok(await _repository.UpdatePrice(price));
        }

    }
}
