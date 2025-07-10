using ShiftTrack.Domain.Common.Interfaces;
using ShiftTrack.Domain.Features.Organization.Structure.Entities;
using ShiftTrack.Domain.Features.System.User.EmployeeRoles.Entities;
using ShiftTrack.Domain.Features.System.User.Employees.Enums;

namespace ShiftTrack.Domain.Features.System.User.Employees.Entities;

public class Employee : ISoftDeletable, IAuditable
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string IntegrationId { get; set; }
    public EmployeeGender Gender { get; set; }
    public string FullName => Surname + " " + Name + " " + Patronymic;
    public string PhotoFullName { get; set; }
    
    public long? DepartmentId { get; set; }
    public Department Department { get; set; }

    public long? PositionId { get; set; }
    public Position Position { get; set; }
    
    public List<EmployeeRole> EmployeeRoles { get; set; }
    
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public long? CreatedById { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public long? ModifiedById { get; set; }
}