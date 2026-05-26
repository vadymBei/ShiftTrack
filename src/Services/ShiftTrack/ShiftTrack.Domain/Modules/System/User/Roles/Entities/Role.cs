using ShiftTrack.Domain.Common.Abstractions;

namespace ShiftTrack.Domain.Modules.System.User.Roles.Entities;

public class Role: AuditableEntity
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
}