using ShiftTrack.Domain.Common.Interfaces;

namespace ShiftTrack.Domain.Features.Organization.Structure.Entities;

public class Department : ISoftDeletable, IAuditable
{
    public long Id { get; set; }
    public string Name { get; set; }

    public long? UnitId { get; set; }
    public Unit Unit { get; set; }
    
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public long? CreatedById { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public long? ModifiedById { get; set; }
}