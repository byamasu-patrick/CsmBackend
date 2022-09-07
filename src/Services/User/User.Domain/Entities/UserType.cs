using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Common;

namespace User.Domain.Entities
{
    public class UserType : BaseDbEntity
    {

        [Column("user_type")]
        public string UserTypeName { get; set; }
    }
}
