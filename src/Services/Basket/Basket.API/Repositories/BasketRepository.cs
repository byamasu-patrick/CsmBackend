using System;
using System.Threading.Tasks;
using Basket.API.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Basket.API.Entities;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;


        public BasketRepository(IDistributedCache cache)
        {
            _redisCache = cache ?? throw new ArgumentNullException(nameof(cache));
        }
        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);

            if (String.IsNullOrEmpty(basket))
                return null;

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }
        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));

            return await GetBasket(basket.UserName);
        }

        public async Task DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }
        // a funtion to delete a specified product in the basket
        public async Task DeleteBasketItem(string userName, string productId)
        {
            var json = await _redisCache.GetStringAsync(userName);

            var basket = JsonConvert.DeserializeObject<ShoppingCart>(json);

           
             var removedItem = basket.Items.RemoveAll(item => item.ProductId == productId);

            var totalPrice = basket.TotalPrice;
            var totalWeight = basket.TotalWeight;

            var updatedBasketJson = JsonConvert.SerializeObject(basket);

            await _redisCache.SetStringAsync(userName, updatedBasketJson);

            await _redisCache.SetStringAsync($"{userName}_totalPrice", totalPrice.ToString());
            await _redisCache.SetStringAsync($"{userName}_totalWeight", totalWeight.ToString());
        }
        // the function to increament the quantity of a specified product in the basket a
        public async Task increaseItemQuantity(string userName, string productId, int value)
        {
            decimal totalPrice;
            decimal totalWeight;
            
            
            var json = await _redisCache.GetStringAsync(userName);

            var basket = JsonConvert.DeserializeObject<ShoppingCart>(json);

      
            var item = basket.Items.Find(item => item.ProductId == productId);
            totalPrice = basket.TotalPrice;
            totalWeight = basket.TotalWeight;

            if (item != null)
            {
               
                item.Quantity += value;

                totalPrice = totalPrice + (item.Price * value);
                totalWeight = totalWeight + (item.Weight * value);
            }
            else
            {
                basket.Items.Add(new ShoppingCartItem { ProductId = productId, Quantity = value });
            }

            var updatedBasketJson = JsonConvert.SerializeObject(basket);

            await _redisCache.SetStringAsync(userName, updatedBasketJson);

            await _redisCache.SetStringAsync($"{userName}_totalPrice", totalPrice.ToString());
            await _redisCache.SetStringAsync($"{userName}_totalWeight", totalWeight.ToString());
        }
        // a function to decrement the quantity of a specified product in the basket 
        public async Task decreaseItemQuantity(string userName, string productId, int value)
        {
            decimal totalPrice;
            decimal totalWeight;


            var json = await _redisCache.GetStringAsync(userName);

            var basket = JsonConvert.DeserializeObject<ShoppingCart>(json);


            var item = basket.Items.Find(item => item.ProductId == productId);
            totalPrice = basket.TotalPrice;
            totalWeight = basket.TotalWeight;

            if (item != null)
            {

                int quantity = item.Quantity -= value;
                if (quantity > 0)
                {
                    totalPrice = totalPrice - (item.Price * value);
                    totalWeight = totalWeight - (item.Weight * value);

                }
                else
                {
                    item.Quantity = 0;
                    totalPrice = totalPrice - item.Price;
                    totalWeight = totalWeight - item.Weight;
                }

            }
            else
            {
                basket.Items.Add(new ShoppingCartItem { ProductId = productId, Quantity = value });
            }

            var updatedBasketJson = JsonConvert.SerializeObject(basket);

            await _redisCache.SetStringAsync(userName, updatedBasketJson);

            await _redisCache.SetStringAsync($"{userName}_totalPrice", totalPrice.ToString());
            await _redisCache.SetStringAsync($"{userName}_totalWeight", totalWeight.ToString());
        }

        // add product in the basket 

        public async Task addProduct(string userName, string productId)
        {
            decimal totalPrice;


            var json = await _redisCache.GetStringAsync(userName);

            var basket = JsonConvert.DeserializeObject<ShoppingCart>(json);
            if (basket != null )
            {
                var item = basket.Items.Find(item => item.ProductId == productId);
                if ( item != null)
                {
                    
                }

            }


        }


    }
}
