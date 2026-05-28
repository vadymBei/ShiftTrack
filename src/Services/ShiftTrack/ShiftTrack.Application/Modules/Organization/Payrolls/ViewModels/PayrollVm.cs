using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.ViewModels;
using ShiftTrack.Domain.Modules.Organization.Payrolls.Entities;
using ShiftTrack.Domain.Modules.Organization.Payrolls.Enums;

namespace ShiftTrack.Application.Modules.Organization.Payrolls.ViewModels;

[AutoMap(typeof(Payroll))]
public class PayrollVm
{
    public long Id { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int WorkedHours { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal TotalAmount { get; set; }
    public PayrollStatus Status { get; set; }
    public DateTime? PaidAt { get; set; }

    public long EmployeeId { get; set; }
    public EmployeeVm Employee { get; set; }

    public IEnumerable<EmployeeShiftVm> EmployeeShifts { get; set; }
}