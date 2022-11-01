using System.ComponentModel.DataAnnotations.Schema;
using User.Domain.Entities;

namespace User.API.Models
{
    public class UserInfos
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public int ProfileId { get; set; }

        public virtual UserProfile Profile { get; set; }
    }
}
