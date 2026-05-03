using AutoMapper;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Enums;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.ViewModels;

[AutoMap(typeof(Shift))]
public class ShiftVm
{
    public long Id { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public TimeSpan? WorkHours { get; set; }
    public ShiftType Type { get; set; }
}