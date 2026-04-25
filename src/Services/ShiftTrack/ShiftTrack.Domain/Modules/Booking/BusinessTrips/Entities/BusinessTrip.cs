using ShiftTrack.Domain.Common.Abstractions;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Enums;

namespace ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;

public class BusinessTrip : AuditableEntity
{
    public long Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Route { get; set; }
    public string Description { get; set; }
    public decimal EstimatedBudget { get; set; }
    public BusinessTripStatus Status { get; set; }

    public List<BusinessTripParticipant> Participants { get; set; }
    public List<BusinessTripLocation> Locations { get; set; }
}