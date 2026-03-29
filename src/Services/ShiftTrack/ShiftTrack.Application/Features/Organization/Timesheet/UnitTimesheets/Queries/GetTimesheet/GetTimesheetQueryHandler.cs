using AutoMapper;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Timesheet.UnitTimesheets.Queries.GetTimesheet;

public class GetTimesheetQueryHandler(
    IMapper mapper,
    ITimesheetService timesheetService) : IRequestHandler<GetTimesheetQuery, TimesheetVm>
{
    public async Task<TimesheetVm> Handle(GetTimesheetQuery request, CancellationToken cancellationToken = default)
    {
        var timesheet = await timesheetService.GetTimesheet(request.Dto, cancellationToken);

        return mapper.Map<TimesheetVm>(timesheet);
    }
}