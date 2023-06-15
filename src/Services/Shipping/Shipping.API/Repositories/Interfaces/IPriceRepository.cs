using Payment.API.Models;
using Shipping.API.Entities;

namespace Shipping.API.Repositories.Interfaces
{
    public interface IPriceRepository
    {
        Task<ShippingResponse<Prices>> GetPrices(int Page);
        Task<Prices> GetPrice(string id);
        Task CreatePrice(Prices price);
        Task<bool> UpdatePrice(Prices price);
        Task<bool> DeletePrice(string id);
    }
}

