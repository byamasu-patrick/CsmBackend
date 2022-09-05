using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Exceptions
{
    public class ActivationTokenExpiredException : TokenException
    {
        public ActivationTokenExpiredException() : base("Token expired")
        {
        }
    }
}
