using AutoMapper;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Payment.API.Entities;
using Payment.API.Models;
using Payment.API.Repositories.Interfaces;
using Payment.API.Services;
using System.Net;

namespace Payment.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICardPaymentRepository _repository;
        private readonly ILogger<BillingController> _logger;
        private IMapper _mapper;

        public BillingController(IConfiguration configuration, ICardPaymentRepository repository,
            ILogger<BillingController> logger, IMapper mapper)
        {
            _configuration = configuration;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{page}")]
        [ProducesResponseType(typeof(PaymentResponse<CardPayment>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaymentResponse<CardPayment>>> GetPayments(int page)
        {
            var payments = await _repository.GetCardPayments(page);
            return Ok(payments);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CardPayment), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CardPayment>> CreateProduct([FromBody] CardPaymentDto cardPaymentDto)
        {
            var cardPayment = _mapper.Map<CardPayment>(cardPaymentDto);

            cardPayment.Id = ObjectId.GenerateNewId().ToString();
            cardPayment.CreatedAt = DateTime.UtcNow;

            await _repository.CreateCardPayment(cardPayment);

            return CreatedAtRoute("GetBilling", new { id = cardPayment.Id }, cardPayment);
        }

        [HttpGet("{id:length(24)}", Name = "GetBilling")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(CardPayment), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CardPayment>> GetBillingById(string id)
        {
            var payment = await _repository.GetCardPayment(id);

            if (payment == null)
            {
                _logger.LogError($"Billing with id: {id}, not found.");
                return NotFound();
            }

            return Ok(payment);
        }

        [Route("[action]/{userId}", Name = "GetBillingByUserId")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CardPayment>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<CardPayment>>> GetBillingByUserId(string userId)
        {
            var payments = await _repository.GetCardPaymentUserId(userId);
            return Ok(payments);
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteBillingById")]
        [ProducesResponseType(typeof(CardPayment), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBillingById(string id)
        {
            return Ok(await _repository.DeleteCardPayment(id));
        }

    }
}
