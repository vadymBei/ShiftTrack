using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.ViewModels.Shifts;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Queries.GetShifts;

public class GetShiftsQueryHandler(
    IMapper mapper,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<GetShiftsQuery, IEnumerable<ShiftVM>>
{
    public async Task<IEnumerable<ShiftVM>> Handle(GetShiftsQuery request, CancellationToken cancellationToken)
    {
        var shifts = await applicationDbContext.Shifts
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return mapper.Map<List<ShiftVM>>(shifts);
    }
}