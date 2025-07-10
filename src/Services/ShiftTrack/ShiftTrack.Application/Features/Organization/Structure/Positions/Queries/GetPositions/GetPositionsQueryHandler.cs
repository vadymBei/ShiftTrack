using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Positions.Queries.GetPositions;

public class GetPositionsQueryHandler(
    IMapper mapper,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<GetPositionsQuery, IEnumerable<PositionVm>>
{
    public async Task<IEnumerable<PositionVm>> Handle(GetPositionsQuery request, CancellationToken cancellationToken)
    {
        var positions = await applicationDbContext.Positions
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return mapper.Map<List<PositionVm>>(positions);
    }
}