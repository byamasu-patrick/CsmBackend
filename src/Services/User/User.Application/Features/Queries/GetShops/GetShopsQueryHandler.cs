using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Contracts.Persistence;

namespace User.Application.Features.Queries.GetShops
{
    public class GetShopsQueryHandler : IRequestHandler<GetShopsQuery, IEnumerable<UserVmShops>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public GetShopsQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<IEnumerable<UserVmShops>> Handle(GetShopsQuery request, CancellationToken cancellationToken)
        {
            var userDate = await _userRepository.GetShops(request.Shops);

            var result = _mapper.Map<IEnumerable<UserVmShops>>(userDate);

            return result;
        }
    }
}
