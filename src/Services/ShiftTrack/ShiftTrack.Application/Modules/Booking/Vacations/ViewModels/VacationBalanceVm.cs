using AutoMapper;
using ShiftTrack.Domain.Modules.Booking.Vacations.Models;

namespace ShiftTrack.Application.Modules.Booking.Vacations.ViewModels;

[AutoMap(typeof(VacationBalance))]
public class VacationBalanceVm
{
    public int TotalDays { get; set; }
    public int UsedDays { get; set; }
    public int PlannedDays { get; set; }
}