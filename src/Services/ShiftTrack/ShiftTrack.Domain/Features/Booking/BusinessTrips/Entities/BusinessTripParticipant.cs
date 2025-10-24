using ShiftTrack.Domain.Common.Abstractions;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;

namespace ShiftTrack.Domain.Features.Booking.BusinessTrips.Entities;

public class BusinessTripParticipant : AuditableEntity
{
    public long Id { get; set; }
    
    public long BusinessTripId { get; set; }
    public BusinessTrip BusinessTrip { get; set; }
    
    public long EmployeeId { get; set; }
    public Employee Employee { get; set; }
}