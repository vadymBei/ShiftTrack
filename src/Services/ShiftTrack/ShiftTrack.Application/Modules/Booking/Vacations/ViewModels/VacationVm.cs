using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;

namespace ShiftTrack.Application.Modules.Booking.Vacations.ViewModels;

[AutoMap(typeof(Vacation))]
public class VacationVm
{
    public long Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime ApprovalDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Comment { get; set; }
    public int DaysCount { get; set; }
    public int DaysBalanceAtCreation { get; set; }
    public VacationType Type { get; set; }
    public VacationStatus Status { get; set; }
    
    public long? AuthorId { get; set; }
    
    public long EmployeeId { get; set; }
    public EmployeeVm Employee { get; set; }

    public long? ApprovedEmployeeId { get; set; }
    public EmployeeVm ApprovedEmployee { get; set; }
}