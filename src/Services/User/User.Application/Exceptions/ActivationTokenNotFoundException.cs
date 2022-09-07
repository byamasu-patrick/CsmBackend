using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Exceptions
{
    public class ActivationTokenNotFoundException : TokenNotFoundException
    {
        public ActivationTokenNotFoundException() : base("Token not found")
        {
        }
    }
}
