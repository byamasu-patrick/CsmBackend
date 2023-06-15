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
    public class LocationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private static string s_wasmClientURL;
        private readonly ILocationRepository _repository;
        private readonly ILogger<LocationController> _logger;
        private IMapper _mapper;

        public LocationController(IConfiguration configuration, ILocationRepository repository,
            ILogger<LocationController> logger, IMapper mapper)
        {
            _configuration = configuration;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{page}")]
        [ProducesResponseType(typeof(ShippingResponse<LocationAddress>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShippingResponse<LocationAddress>>> Locations(int page)
        {
            var locations = await _repository.GetLocations(page);
            return Ok(locations);
        }

        [HttpGet("{id:length(24)}", Name = "GetLocationById")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(LocationAddress), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<LocationAddress>> GetLocationById(string id)
        {
            var location = await _repository.GetLocation(id);

            if (location == null)
            {
                _logger.LogError($"Location  with id: {id}, not found.");
                return NotFound();
            }

            return Ok(location);
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteLocationById")]
        [ProducesResponseType(typeof(Courier), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteLocationId(string id)
        {
            return Ok(await _repository.DeleteLocation(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(LocationAddress), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<LocationAddress>> CreateLocation([FromBody] CreateLocationAddressDto locationDto)
        {
            var location = _mapper.Map<LocationAddress>(locationDto);

            location.Id = ObjectId.GenerateNewId().ToString();

            await _repository.CreateLocation(location);

            return Ok(location);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Courier), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateLocation([FromBody] LocationAddress location)
        {
            return Ok(await _repository.UpdateLocation(location));
        }

    }
}
