using ShiftTrack.Application.Modules.Organization.Timesheet.Common.Dtos;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Models;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.Common.Interfaces;

public interface ITimesheetService
{
    Task<UnitTimesheet> GetTimesheet(TimesheetDto dto, CancellationToken cancellationToken);
}