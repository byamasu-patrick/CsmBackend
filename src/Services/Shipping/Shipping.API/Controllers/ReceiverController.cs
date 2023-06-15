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
    public class ReceiverController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private static string s_wasmClientURL;
        private readonly IReceiverRepository _repository;
        private readonly ILogger<ReceiverController> _logger;
        private IMapper _mapper;

        public ReceiverController(IConfiguration configuration, IReceiverRepository repository,
            ILogger<ReceiverController> logger, IMapper mapper)
        {
            _configuration = configuration;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{page}")]
        [ProducesResponseType(typeof(ShippingResponse<Receiver>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShippingResponse<Receiver>>> Receivers(int page)
        {
            var couriers = await _repository.GetReceivers(page);
            return Ok(couriers);
        }

        [HttpGet("{id:length(24)}", Name = "GetReceiverById")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Receiver), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Receiver>> GetReceiverById(string id)
        {
            var receiver = await _repository.GetReceiver(id);

            if (receiver == null)
            {
                _logger.LogError($"Receiver  with id: {id}, not found.");
                return NotFound();
            }

            return Ok(receiver);
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteReceiverById")]
        [ProducesResponseType(typeof(Courier), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteReceiverId(string id)
        {
            return Ok(await _repository.DeleteReceiver(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Receiver), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Receiver>> CreateReceiver([FromBody] CreateReceiverDto receiverDto)
        {
            var receiver = _mapper.Map<Receiver>(receiverDto);

            receiver.Id = ObjectId.GenerateNewId().ToString();

            await _repository.CreateReceiver(receiver);

            return Ok(receiver);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Receiver), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateReceiver([FromBody] Receiver receiver)
        {
            return Ok(await _repository.UpdateReceiver(receiver));
        }

    }
}
