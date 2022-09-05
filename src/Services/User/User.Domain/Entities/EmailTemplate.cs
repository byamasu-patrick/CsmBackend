using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Common;

namespace User.Domain.Entities
{
    public class EmailTemplate : BaseDbEntity
    {
        public string TemplateContent { get; set; }

        [ForeignKey(nameof(TemplateType))]
        public int TemplateTypeId { get; set; }

        public bool IsActive { get; set; }
        public virtual TemplateType TemplateType { get; set; }
    }
}
