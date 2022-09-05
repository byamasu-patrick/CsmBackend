using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Common;

namespace User.Domain.Entities
{
    public class UserProfile : BaseDbEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
