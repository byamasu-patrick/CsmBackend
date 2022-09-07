using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Common;

namespace User.Domain.Entities
{
    public class TemplateType : BaseDbEntity
    {
        public string Type { get; set; }
    }
}
