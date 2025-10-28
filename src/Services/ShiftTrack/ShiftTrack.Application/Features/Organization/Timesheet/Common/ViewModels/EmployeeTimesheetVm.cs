using AutoMapper;
using ShiftTrack.Application.Features.Organization.Employees.Common.ViewModels;
using ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Models;

namespace ShiftTrack.Application.Features.Organization.Timesheet.Common.ViewModels;

[AutoMap(typeof(EmployeeTimesheet))]
public class EmployeeTimesheetVm
{
    public EmployeeVm Employee { get; set; }
    public int TotalWorkDays { get; set; }
    public int TotalWorkHours { get; set; }
    public IEnumerable<EmployeeShiftVm> EmployeeShifts { get; set; }
}