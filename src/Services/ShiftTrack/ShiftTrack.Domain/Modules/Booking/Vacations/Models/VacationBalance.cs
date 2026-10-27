namespace ShiftTrack.Domain.Modules.Booking.Vacations.Models;

public class VacationBalance
{
    /// <summary>Annual vacation length per working year.</summary>
    public int AnnualDays { get; set; }

    /// <summary>All days accrued since the employment start, unused days carry over.</summary>
    public decimal TotalDays { get; set; }

    /// <summary>Approved annual vacations that have already started.</summary>
    public int UsedDays { get; set; }

    /// <summary>Approved annual vacations that haven't started yet.</summary>
    public int PlannedDays { get; set; }

    public decimal AvailableDays { get; set; }

    public DateTime? WorkingYearStart { get; set; }
    public DateTime? WorkingYearEnd { get; set; }

    /// <summary>First day annual vacation can start (after 6 months of continuous work).</summary>
    public DateTime AnnualVacationAvailableFrom { get; set; }
}
