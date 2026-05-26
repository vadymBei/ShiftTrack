using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.UseCases.Queries.GetUnitTimesheet;

public class GetTimesheetQueryHandler(
    IMapper mapper,
    IUnitTimesheetService unitTimesheetService) : IRequestHandler<GetTimesheetQuery, UnitTimesheetVm>
{
    public async Task<UnitTimesheetVm> Handle(GetTimesheetQuery request, CancellationToken cancellationToken = default)
    {
        var timesheet = await unitTimesheetService.GetTimesheet(request.Dto, cancellationToken);

        return mapper.Map<UnitTimesheetVm>(timesheet);
    }
}