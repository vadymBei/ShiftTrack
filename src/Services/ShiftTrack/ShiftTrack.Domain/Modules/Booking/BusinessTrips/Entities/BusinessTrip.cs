using ShiftTrack.Domain.Common.Abstractions;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Enums;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;

public class BusinessTrip : AuditableEntity
{
    public long Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }
    public decimal EstimatedBudget { get; set; }
    public BusinessTripStatus Status { get; set; }

    public ICollection<Employee> Participants { get; set; }
    public ICollection<BusinessTripLocation> Locations { get; set; }
}