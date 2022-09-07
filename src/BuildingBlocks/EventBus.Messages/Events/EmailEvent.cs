using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class EmailEvent
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set;}
    }
}
