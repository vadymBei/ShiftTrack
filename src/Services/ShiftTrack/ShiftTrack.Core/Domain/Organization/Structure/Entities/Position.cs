using ShiftTrack.Data.Interfaces;

namespace ShiftTrack.Core.Domain.Organization.Structure.Entities;

public class Position : ISoftDeletable, IAuditable
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public long? CreatedById { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public long? ModifiedById { get; set; }
}