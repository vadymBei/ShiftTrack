using AutoMapper;
using ShiftTrack.Application.Features.Organization.Employees.Common.ViewModels;
using ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Entities;

namespace ShiftTrack.Application.Features.Organization.Timesheet.Common.ViewModels;

[AutoMap(typeof(EmployeeShiftHistory))]
public class EmployeeShiftHistoryVm
{
    public long Id { get; set; }
    public TimeSpan? PreviousStartTime { get; set; }
    public TimeSpan? PreviousEndTime { get; set; }
    public TimeSpan? NewStartTime { get; set; }
    public TimeSpan? NewEndTime { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public long EmployeeShiftId { get; set; }
    public EmployeeShiftVm EmployeeShift { get; set; }

    public long? PreviousShiftId { get; set; }
    public ShiftVm PreviousShift { get; set; }
    
    public long? NewShiftId { get; set; }
    public ShiftVm NewShift { get; set; }
    
    public EmployeeVm Author { get; set; }
}