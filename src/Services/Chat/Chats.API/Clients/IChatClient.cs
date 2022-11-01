using Chats.API.Models;

namespace Chats.API.Clients
{
    public interface IChatClient
    {
        Task ReceiveMessage(ChatMessage message);
    }
}
