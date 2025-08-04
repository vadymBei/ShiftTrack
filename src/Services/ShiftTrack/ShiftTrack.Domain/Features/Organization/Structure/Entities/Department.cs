using ShiftTrack.Domain.Common.Abstractions;
using ShiftTrack.Domain.Common.Interfaces;

namespace ShiftTrack.Domain.Features.Organization.Structure.Entities;

public class Department : AuditableEntity, ISoftDeletable
{
    public long Id { get; set; }
    public string Name { get; set; }

    public long? UnitId { get; set; }
    public Unit Unit { get; set; }
    
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}