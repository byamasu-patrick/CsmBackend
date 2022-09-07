using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Exceptions
{
    public abstract class TokenNotFoundException : Exception
    {
        protected TokenNotFoundException(string message) : base(message)
        {
        }
    }
}
