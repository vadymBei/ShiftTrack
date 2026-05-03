using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.ViewModels;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Entities;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.ViewModels;

[AutoMap(typeof(EmployeeShift))]
public class EmployeeShiftVm
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }

    public long EmployeeId { get; set; }

    public long ShiftId { get; set; }
    public ShiftVm Shift { get; set; }
}