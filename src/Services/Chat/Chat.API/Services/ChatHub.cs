using Chat.API.Clients;
using Chat.API.Models;
using Microsoft.AspNetCore.SignalR;

namespace Chat.API.Services
{
    public class ChatHub : Hub<IChatClient>
    {
        //    public async Task SendMessage(ChatMessage message)
        //    {
        //        await Clients.All.ReceiveMessage(message);
        //    }
    }
}
