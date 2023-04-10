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
    public class CourierController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private static string s_wasmClientURL;
        private readonly ICourierRepository _repository;
        private readonly ILogger<CourierController> _logger;
        private IMapper _mapper;

        public CourierController(IConfiguration configuration, ICourierRepository repository,
            ILogger<CourierController> logger, IMapper mapper)
        {
            _configuration = configuration;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{page}")]
        [ProducesResponseType(typeof(ShippingResponse<Courier>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShippingResponse<Courier>>>Couriers(int page)
        {
            var couriers = await _repository.GetCouriers(page);
            return Ok(couriers);
        }

        [HttpGet("{id:length(24)}", Name = "GetCourierById")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Courier), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Courier>> GetCourierById(string id)
        {
            var courier = await _repository.GetCourier(id);

            if (courier == null)
            {
                _logger.LogError($"Courier  with id: {id}, not found.");
                return NotFound();
            }

            return Ok(courier);
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteCourierById")]
        [ProducesResponseType(typeof(Courier), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteCourierId(string id)
        {
            return Ok(await _repository.DeleteCourier(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Courier), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Courier>> CreateCourier([FromBody] CreateCourierDto courierDto)
        {
            var courier = _mapper.Map<Courier>(courierDto);

            courier.Id = ObjectId.GenerateNewId().ToString();
            courier.CreatedDate = DateTime.UtcNow;

            await _repository.CreateCourier(courier);

            return Ok(courier);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Courier), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateCourier([FromBody] Courier courier)
        {
            return Ok(await _repository.UpdateCourier(courier));
        }

    }
}
