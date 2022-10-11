using Payment.API.Models;
using Shipping.API.Entities;

namespace Shipping.API.Repositories.Interfaces
{
    public interface IShippingMethodRepository
    {
        Task<ShippingResponse<ShippingMethods>> GetShippingMethods(int Page);
        Task<ShippingMethods> GetShippingMethod(string id);
        Task<IEnumerable<ShippingMethods>> GetShippingMethodUserId(string id);
        Task CreateShippingMethod(ShippingMethods shipping);
        Task<bool> UpdateShippingMethod(ShippingMethods shipping);
        Task<bool> DeleteShippingMethod(string id);

    }
}
