using Chats.API.Clients;
using Chats.API.Models;
using Chats.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Chats.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        //private readonly IHubContext<ChatHub> _chatHub;

        //public ChatController(IHubContext<ChatHub> chatHub)
        //{
        //    _chatHub = chatHub;
        //}

        //[HttpPost("messages")]
        //public async Task AddMessage(ChatMessage message)
        //{
        //    // run some logic...

        //    await _chatHub.(message);
        //}

        //[HttpPost("messages")]
        //public async Task Post(ChatMessage message)
        //{
        //    // run some logic...

        //    await _chatHub.(message);
        //}
    }
}
