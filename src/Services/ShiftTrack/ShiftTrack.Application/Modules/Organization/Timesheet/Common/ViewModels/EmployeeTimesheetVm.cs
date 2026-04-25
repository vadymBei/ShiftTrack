using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Employees.Common.ViewModels;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Models;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.Common.ViewModels;

[AutoMap(typeof(EmployeeTimesheet))]
public class EmployeeTimesheetVm
{
    public EmployeeVm Employee { get; set; }
    public int TotalWorkDays { get; set; }
    public int TotalWorkHours { get; set; }
    public IEnumerable<EmployeeShiftVm> EmployeeShifts { get; set; }
}