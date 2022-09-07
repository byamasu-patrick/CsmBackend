using System.ComponentModel.DataAnnotations.Schema;
using User.Domain.Entities;

namespace User.Application.Features.Commands.ActivateUser
{
    public class UserDataVm
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }

        [ForeignKey(nameof(UserType))]
        public int UserTypeId { get; set; }

        [ForeignKey(nameof(Profile))]
        public int ProfileId { get; set; }

        public virtual UserType UserType { get; set; }
        public virtual UserProfile Profile { get; set; }
    }
}