using ShiftTrack.Domain.Common.Abstractions;
using ShiftTrack.Domain.Common.Interfaces;

namespace ShiftTrack.Domain.Features.Organization.Structure.Entities;

public class Unit : AuditableEntity, ISoftDeletable
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Code { get; set; }

    public string FullName
    {
        get
        {
            return Code + " " + Name;
        }
    }
    
    public List<Department> Departments { get; set; }
    
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}