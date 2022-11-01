using Chat.API.Models;

namespace Chat.API.Clients
{
    public interface IChatClient
    {
        Task ReceiveMessage(ChatMessage message);
    }
}
