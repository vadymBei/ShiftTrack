using AutoMapper;
using ShiftTrack.Domain.Features.Booking.Vacations.Entities;
using ShiftTrack.Domain.Features.Booking.Vacations.Enums;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;

namespace ShiftTrack.Application.Features.Booking.Common.ViewModels;

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

    public long EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public long? ApprovedEmployeeId { get; set; }
    public Employee ApprovedEmployee { get; set; }
}