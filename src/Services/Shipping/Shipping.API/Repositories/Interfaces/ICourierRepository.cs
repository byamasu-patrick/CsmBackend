using Payment.API.Models;
using Shipping.API.Entities;

namespace Shipping.API.Repositories.Interfaces
{
    public interface ICourierRepository
    {

        Task<ShippingResponse<Courier>> GetCouriers(int Page);
        Task<Courier> GetCourier(string id);
        Task CreateCourier(Courier courier);
        Task<bool> UpdateCourier(Courier courier);
        Task<bool> DeleteCourier(string id);
    }
}
