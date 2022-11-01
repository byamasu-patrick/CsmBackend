using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Features.Queries.GetShops
{
    public class GetShopsQuery : IRequest<IEnumerable<UserVmShops>>
    {
        public bool Shops { get; set; }
        public GetShopsQuery(bool shops)
        {
            Shops = shops;
        }
    }
}
