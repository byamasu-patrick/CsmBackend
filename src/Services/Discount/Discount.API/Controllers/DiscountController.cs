using Discount.API.Entities;
using Discount.API.Models;
using Discount.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Powells.CouponCode;
using System.Net;

namespace Discount.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _repository;

        public DiscountController(IDiscountRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("{productName}", Name = "GetDiscount")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName)
        {
            var discount = await _repository.GetDiscount(productName);
            return Ok(discount);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ActionResult<IEnumerable<Coupon>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Coupon>>> GetDiscounts()
        {
            var discount = await _repository.GetDiscounts();
            return Ok(discount);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] CouponDto coupon)
        {
            var couponDiscount = new Coupon();

            couponDiscount.ProductName = coupon.ProductName;
            couponDiscount.Description = coupon.Description;
            couponDiscount.ProductId = coupon.ProductId;
            couponDiscount.Headline = coupon.Headline;
            couponDiscount.CouponCode = GenerateCodes();
            couponDiscount.Amount = coupon.Amount;

            await _repository.CreateDiscount(couponDiscount);
            return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
        }

        private string GenerateCodes()
        {
            var opts = new Options();
            var ccb = new CouponCodeBuilder();
            var badWords = ccb.BadWordsList;
            var code = ccb.Generate(opts);

            return code;
        }

        [HttpPut]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> UpdateBasket([FromBody] Coupon coupon)
        {
            return Ok(await _repository.UpdateDiscount(coupon));
        }

        [HttpDelete("{productName}", Name = "DeleteDiscount")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteDiscount(string productName)
        {
            return Ok(await _repository.DeleteDiscount(productName));
        }

    }
}
