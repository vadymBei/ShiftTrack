using ShiftTrack.Domain.Common.Abstractions;

namespace ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;

public class BusinessTripLocation : AuditableEntity
{
    public long Id { get; set; }
    
    public long BusinessTripId { get; set; }
    public BusinessTrip BusinessTrip { get; set; }
    
    public string LocationIntegrationId { get; set; }
}