using Payment.API.Models;
using Shipping.API.Entities;

namespace Shipping.API.Repositories.Interfaces
{
    public interface IShippingAddressRepository
    {
        Task<ShippingResponse<ShippingAddress>> GetShippingAddresses(int Page);
        Task<ShippingAddress> GetShippingAddress(string id);
        Task<IEnumerable<ShippingAddress>> GetShippingAddressUserId(string id);
        Task CreateShippingAddress(ShippingAddress shipping);
        Task<bool> UpdateShippingAddress(ShippingAddress shipping);
        Task<bool> DeleteShippingAddress(string id);

    }
}
