using ShiftTrack.Application.Features.Organization.Timesheet.Common.Dtos;
using ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Models;

namespace ShiftTrack.Application.Features.Organization.Timesheet.Common.Interfaces;

public interface ITimesheetService
{
    Task<UnitTimesheet> GetTimesheet(TimesheetDto dto, CancellationToken cancellationToken);
}