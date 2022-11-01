using Chats.API.Clients;
using Chats.API.Models;
using Microsoft.AspNetCore.SignalR;

namespace Chats.API.Services
{
    public class ChatHub : Hub
    {
        private readonly string _botUser;
        private readonly IDictionary<string, UserConnection> _connection;
        public ChatHub(IDictionary<string, UserConnection> connection)
        {
            _botUser = "MyChat Bot";
            _connection = connection;
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (_connection.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                _connection.Remove(Context.ConnectionId);
                Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", _botUser, new MessageResponse
                {
                    User = userConnection.User,
                    Message = $"{userConnection.User} has left"
                });                    
                SendUsersConnected(userConnection.Room);
            }

            return base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessage(string chatMessage)
        {
            if (_connection.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                await Clients.Group(userConnection.Room).SendAsync(
                    "ReceiveMessage", userConnection.User, new MessageResponse
                    {
                        User = userConnection.User,
                        Message = chatMessage
                    }
                );
            }
        }
        public async Task JoinRoom(UserConnection connection)
        {
            //Console.WriteLine(connection.Room);
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.Room);

            _connection[Context.ConnectionId] = connection;

            await Clients.Group(connection.Room).SendAsync("ReceiveMessage", _botUser, new MessageResponse
            {
                User = connection.User,
                Message = $"{connection.User.Name} has joined {connection.Room}"
            } );
            
            await SendUsersConnected(connection.Room);

        }
        public Task SendUsersConnected(string room)
        {
            var users = _connection.Values
                .Where(c => c.Room == room)
                .Select(c => c.User);

            return Clients.Group(room).SendAsync("UsersInRoom", users);
        }

    }
}
