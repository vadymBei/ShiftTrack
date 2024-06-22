using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using ShiftTrack.Core.Application.Data.Common.Exceptions;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Positions.Queries.GetPositions
{
    public class GetPositionsQueryHandler : IRequestHandler<GetPositionsQuery, IEnumerable<PositionVM>>
    {
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        private readonly IApplicationDbContext _applicationDbContext;

        public GetPositionsQueryHandler(
            IMapper mapper,
            IDistributedCache distributedCache,
            IApplicationDbContext applicationDbContext)
        {
            _mapper = mapper;
            _distributedCache = distributedCache;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<PositionVM>> Handle(GetPositionsQuery request, CancellationToken cancellationToken)
        {
            var positions = await _distributedCache.GetOrCreateAsync(
                "positions",
                async () =>
                {
                   return await _applicationDbContext.Positions
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
                },
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
                });

            return _mapper.Map<List<PositionVM>>(positions);
        }
    }
}
