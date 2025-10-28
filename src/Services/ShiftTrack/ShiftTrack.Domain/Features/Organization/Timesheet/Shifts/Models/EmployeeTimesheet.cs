using ShiftTrack.Domain.Common.Extensions;
using ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Enums;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;

namespace ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Models;

public class EmployeeTimesheet
{
    public Employee Employee { get; set; }

    public int TotalWorkDays
    {
        get { return EmployeeShifts.Count(x => x.Shift.Type == ShiftType.Workday); }
    }

    public int TotalWorkHours
    {
        get
        {
            var totalWorkTime = EmployeeShifts
                .Where(x => x.Shift.WorkHours is not null)
                .Select(x => x.Shift.WorkHours.Value)
                .Sum();

            return (int)totalWorkTime.TotalHours;
        }
    }

    public IEnumerable<EmployeeShift> EmployeeShifts { get; set; }
}