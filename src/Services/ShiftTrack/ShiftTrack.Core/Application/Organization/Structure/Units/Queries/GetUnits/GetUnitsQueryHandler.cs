using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Queries.GetUnits
{
    public class GetUnitsQueryHandler : IRequestHandler<GetUnitsQuery, IEnumerable<UnitVM>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _dbContext;

        public GetUnitsQueryHandler(
            IMapper mapper,
            IApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<UnitVM>> Handle(GetUnitsQuery request, CancellationToken cancellationToken)
        {
            var units = await _dbContext.Units
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<UnitVM>>(units);
        }
    }
}
