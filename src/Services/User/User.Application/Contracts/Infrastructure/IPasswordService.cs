using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Contracts.Infrastructure
{
    public interface IPasswordService
    {
        (string hash, string salt) HashPassword(string password);
        bool ComparePasswordWithHash(string hash, string salt, string password);
    }
}
