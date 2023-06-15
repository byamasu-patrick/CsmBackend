using Payment.API.Models;
using Shipping.API.Entities;

namespace Shipping.API.Repositories.Interfaces
{
    public interface IReceiverRepository
    {
        Task<ShippingResponse<Receiver>> GetReceivers(int Page);
        Task<Receiver> GetReceiver(string id);
        Task CreateReceiver(Receiver receiver);
        Task<bool> UpdateReceiver(Receiver receiver);
        Task<bool> DeleteReceiver(string id);
    }
}

