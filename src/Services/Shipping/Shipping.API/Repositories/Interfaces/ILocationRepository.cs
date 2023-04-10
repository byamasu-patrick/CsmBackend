using Payment.API.Models;
using Shipping.API.Entities;

namespace Shipping.API.Repositories.Interfaces
{
    public interface ILocationRepository
    {
        Task<ShippingResponse<LocationAddress>> GetLocations(int Page);
        Task<LocationAddress> GetLocation(string id);
        Task CreateLocation(LocationAddress location);
        Task<bool> UpdateLocation(LocationAddress location);
        Task<bool> DeleteLocation(string id);
    }
}

