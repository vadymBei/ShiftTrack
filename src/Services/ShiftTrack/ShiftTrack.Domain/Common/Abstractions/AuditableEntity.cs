using ShiftTrack.Domain.Features.System.User.Employees.Entities;

namespace ShiftTrack.Domain.Common.Abstractions;

public abstract class AuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    
    public long? AuthorId { get; set; }
    public Employee Author { get; set; }
    
    public long? ModifierId { get; set; }
    public Employee Modifier { get; set; }
}
