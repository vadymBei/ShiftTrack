using AutoMapper;
using ShiftTrack.Domain.Modules.Booking.Vacations.Models;

namespace ShiftTrack.Application.Modules.Booking.Vacations.ViewModels;

[AutoMap(typeof(VacationBalance))]
public class VacationBalanceVm
{
    public int AnnualDays { get; set; }
    public decimal TotalDays { get; set; }
    public int UsedDays { get; set; }
    public int PlannedDays { get; set; }
    public decimal AvailableDays { get; set; }
    public DateTime? WorkingYearStart { get; set; }
    public DateTime? WorkingYearEnd { get; set; }

    /// <summary>First day annual vacation can start (after 6 months of continuous work).</summary>
    public DateTime AnnualVacationAvailableFrom { get; set; }
}
