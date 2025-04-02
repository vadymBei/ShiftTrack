using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Positions.Queries.GetPositions;

public class GetPositionsQueryHandler(
    IMapper mapper,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<GetPositionsQuery, IEnumerable<PositionVM>>
{
    public async Task<IEnumerable<PositionVM>> Handle(GetPositionsQuery request, CancellationToken cancellationToken)
    {
        var positions = await applicationDbContext.Positions
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return mapper.Map<List<PositionVM>>(positions);
    }
}