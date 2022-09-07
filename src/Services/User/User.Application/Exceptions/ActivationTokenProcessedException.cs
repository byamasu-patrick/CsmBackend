using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Exceptions
{
    public class ActivationTokenProcessedException : TokenException
    {
        public ActivationTokenProcessedException() : base("Token is already processed")
        {
        }
    }
}
