using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Units.Queries.GetUnits;

public class GetUnitsQueryHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<GetUnitsQuery, IEnumerable<UnitVm>>
{
    public async Task<IEnumerable<UnitVm>> Handle(GetUnitsQuery request, CancellationToken cancellationToken)
    {
        var units = await dbContext.Units
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return mapper.Map<List<UnitVm>>(units);
    }
}