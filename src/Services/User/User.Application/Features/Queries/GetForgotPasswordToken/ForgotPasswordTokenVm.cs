using User.Domain.Entities;

namespace User.Application.Features.Queries.GetForgotPasswordToken
{
    public class ForgotPasswordTokenVm
    {
        public string EmailAddress { get; set; }
        public string Token { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}