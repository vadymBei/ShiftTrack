using ShiftTrack.Domain.Common.Interfaces;

namespace ShiftTrack.Domain.Features.System.User.Roles.Entities;

public class Role: IAuditable
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public long? CreatedById { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public long? ModifiedById { get; set; }
}