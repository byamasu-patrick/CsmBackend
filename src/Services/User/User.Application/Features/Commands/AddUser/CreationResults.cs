using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entities;

namespace User.Application.Features.Commands.AddUser
{
    public class CreationResults<T> where T: UserData
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }

        public T Data { get; set; }
    }
}
