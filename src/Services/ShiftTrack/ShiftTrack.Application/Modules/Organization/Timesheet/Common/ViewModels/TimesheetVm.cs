using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Common.ViewModels;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Models;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.Common.ViewModels;

[AutoMap(typeof(UnitTimesheet))]
public class TimesheetVm
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DepartmentVm Department { get; set; }
    public List<EmployeeTimesheetVm> EmployeeTimesheets { get; set; } = [];
}