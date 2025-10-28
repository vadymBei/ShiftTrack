using ShiftTrack.Domain.Features.Organization.Structure.Entities;

namespace ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Models;

public class UnitTimesheet
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Department Department { get; set; }
    public List<EmployeeTimesheet> EmployeeTimesheets { get; set; } = [];
}