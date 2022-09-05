using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Common;

namespace User.Domain.Entities
{
    public abstract class ApplicationToken : BaseDbEntity
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
