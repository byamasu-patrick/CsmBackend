using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment.API.Entities;
using Payment.API.Services;
using Stripe;
using AutoMapper;
using Payment.API.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net;
using Payment.API.Models;
using MongoDB.Bson;
using Stripe.Checkout;

namespace Payment.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private static string s_wasmClientURL;
        private readonly IPaymentService _paymentService;
        private readonly IPaymentTransactionRepository _repository;
        private readonly ILogger<PaymentController> _logger;
        private IMapper _mapper;

        public PaymentController(IConfiguration configuration, IPaymentService paymentService, IPaymentTransactionRepository repository,
            ILogger<PaymentController> logger, IMapper mapper)
        {
            _configuration = configuration;
            _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet("{page}")]
        [ProducesResponseType(typeof(PaymentResponse<PaymentTransaction>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaymentResponse<PaymentTransaction>>> GetPayments(int page)
        {
            var payments = await _repository.GetPaymentTransactions(page);
            return Ok(payments);
        }
        [HttpGet("{id:length(24)}", Name = "GetPaymentById")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PaymentTransaction), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaymentTransaction>> GetPaymentById(string id)
        {
            var payment = await _repository.GetPaymentTransaction(id);

            if (payment == null)
            {
                _logger.LogError($"Payment with id: {id}, not found.");
                return NotFound();
            }

            return Ok(payment);
        }

        [Route("[action]/{userId}", Name = "GetPaymentByUserId")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PaymentTransaction>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<PaymentTransaction>>> GetPaymentByUserId(string userId)
        {
            var payments = await _repository.GetCardPaymentTransactionUserId(userId);
            return Ok(payments);
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProductById")]
        [ProducesResponseType(typeof(PaymentTransaction), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            return Ok(await _repository.CancelPaymentTransaction(id));
        }

        [HttpPost]
        public async Task<ActionResult> CheckoutOrder([FromBody] PaymentTransaction payment, [FromServices] IServiceProvider sp)
        {
            var referer = Request.Headers.Referer;
            s_wasmClientURL = referer[0];

            // Build the URL to which the customer will be redirected after paying.
            var server = sp.GetRequiredService<IServer>();

            var serverAddressesFeature = server.Features.Get<IServerAddressesFeature>();

            string? thisApiUrl = null;

            if (serverAddressesFeature is not null)
            {
                thisApiUrl = serverAddressesFeature.Addresses.FirstOrDefault();
            }

            if (thisApiUrl is not null)
            {
                var paymentTransaction = await _paymentService.CheckOut(payment, thisApiUrl, s_wasmClientURL);

                paymentTransaction.Id = ObjectId.GenerateNewId().ToString();
                await _repository.CheckoutPaymentTransaction(paymentTransaction);

                return Ok(paymentTransaction);
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpGet("success")]
        // Automatic query parameter handling from ASP.NET.
        // Example URL: https://localhost:7051/checkout/success?sessionId=si_123123123123
        public ActionResult CheckoutSuccess(string sessionId)
        {
            var sessionService = new SessionService();
            var session = sessionService.Get(sessionId);

            // Here you can save order and customer details to your database.
            var total = session.AmountTotal.Value;
            var customerEmail = session.CustomerDetails.Email;

            return Redirect(s_wasmClientURL + "success");
        }
    }
}
