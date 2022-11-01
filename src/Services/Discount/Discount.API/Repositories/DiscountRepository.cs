using Dapper;
using Discount.API.Entities;
using Discount.API.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Threading.Tasks;


namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

            if (coupon == null)
                return new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc", CouponCode = "No Coupon Code", Headline = "No Headline", ProductId = "No Product Id", Id = 0 };
            return coupon;
        }

        public async Task<IEnumerable<Coupon>> GetDiscounts()
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var coupon = await connection.QueryAsync<Coupon>
                ("SELECT * FROM Coupon");

            if (coupon == null)
                return null;
            return coupon.ToList();
        }
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected =
                await connection.ExecuteAsync
                    ("INSERT INTO Coupon (ProductName, Description, ProductId, CouponCode, Headline, Amount) VALUES (@ProductName, @Description, @ProductId, @CouponCode, @Headline, @Amount)",
                            new { ProductName = coupon.ProductName, Description = coupon.Description, ProductId = coupon.ProductId, CouponCode = coupon.CouponCode, Headline = coupon.Headline, Amount = coupon.Amount });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                    ("UPDATE Coupon SET ProductName=@ProductName, Description = @Description, ProductId=@ProductId, CouponCode=@CouponCode, Headline=@Headline,  Amount = @Amount WHERE Id = @Id",
                            new { ProductName = coupon.ProductName, Description = coupon.Description, ProductId = coupon.ProductId, CouponCode = coupon.CouponCode, Headline = coupon.Headline, Amount = coupon.Amount, Id = coupon.Id });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });

            if (affected == 0)
                return false;

            return true;
        }
    }
}
