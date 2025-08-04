using ShiftTrack.Domain.Common.Abstractions;
using ShiftTrack.Domain.Common.Interfaces;

namespace ShiftTrack.Domain.Features.Organization.Structure.Entities;

public class Position : AuditableEntity, ISoftDeletable
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}