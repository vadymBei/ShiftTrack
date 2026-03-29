using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Timesheet.Shifts.Queries.GetShifts;

public class GetShiftsQueryHandler(
    IMapper mapper,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<GetShiftsQuery, IEnumerable<ShiftVm>>
{
    public async Task<IEnumerable<ShiftVm>> Handle(GetShiftsQuery request, CancellationToken cancellationToken)
    {
        var shifts = await applicationDbContext.Shifts
            .AsNoTracking()
            .OrderBy(x => x.Code)
            .ToListAsync(cancellationToken);

        return mapper.Map<List<ShiftVm>>(shifts);
    }
}