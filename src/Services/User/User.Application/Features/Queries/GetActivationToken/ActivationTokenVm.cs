using User.Domain.Entities;

namespace User.Application.Features.Queries.GetActivationToken
{
    public class ActivationTokenVm
    {
        public string EmailAddress { get; set; }
        public string Token { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}