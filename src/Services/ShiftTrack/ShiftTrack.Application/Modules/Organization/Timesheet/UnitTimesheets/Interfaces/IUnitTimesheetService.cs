using ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.Dtos;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Models;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.Interfaces;

public interface IUnitTimesheetService
{
    Task<UnitTimesheet> GetTimesheet(UnitTimesheetDto dto, CancellationToken cancellationToken);
}