using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Queries.GetUnits;

public class GetUnitsQueryHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<GetUnitsQuery, IEnumerable<UnitVM>>
{
    public async Task<IEnumerable<UnitVM>> Handle(GetUnitsQuery request, CancellationToken cancellationToken)
    {
        var units = await dbContext.Units
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return mapper.Map<List<UnitVM>>(units);
    }
}