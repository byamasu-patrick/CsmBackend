namespace Chats.API.Models
{
    public class UserConnection
    {
        public User User { get; set; }
        public string Room { get; set; }
        //public DateTime Created { get; set; } = DateTime.UtcNow;
    }
    public class MessageResponse
    {
        public User User { get; set; }
        public string Message { get; set; }
    }
}
