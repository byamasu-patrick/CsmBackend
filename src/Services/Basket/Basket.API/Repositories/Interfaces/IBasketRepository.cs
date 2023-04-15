using Basket.API.Entities;

namespace Basket.API.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string userName);
        Task<ShoppingCart> UpdateBasket(ShoppingCart basket);
        Task DeleteBasket(string userName);
        Task DeleteBasketItem(string userName, string ProductId);
        Task increaseItemQuantity(string userName, string ProductId, int value);
        Task decreaseItemQuantity(string userName, string ProductId, int value);
       
    }
}
