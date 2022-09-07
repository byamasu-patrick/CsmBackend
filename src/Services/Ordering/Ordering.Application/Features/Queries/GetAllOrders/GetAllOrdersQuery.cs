using MediatR;
using Ordering.Application.Features.Queries.GetOrdersList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Queries.GetAllOrders
{
    public class GetAllOrdersQuery : IRequest<List<OrdersVm>>
    {
        
    }
}
