using ShiftTrack.Domain.Common.Abstractions;
using ShiftTrack.Domain.Common.Interfaces;
using ShiftTrack.Domain.Modules.Organization.Structure.Entities;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;
using ShiftTrack.Domain.Modules.System.User.Employees.Enums;

namespace ShiftTrack.Domain.Modules.System.User.Employees.Entities;

public class Employee : AuditableEntity, ISoftDeletable
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

    /// <summary>
    /// The first day of work, the start of the length of service for vacations.
    /// Not set for employees registered before it was stored: the registration date is used then.
    /// </summary>
    public DateTime? HireDate { get; set; }

    /// <summary>The start of the length of service: the hire date, or the registration date if it isn't set.</summary>
    public DateTime ServiceStart => ServiceStartOf(HireDate, CreatedAt);

    public static DateTime ServiceStartOf(DateTime? hireDate, DateTime createdAt) =>
        hireDate?.Date ?? (createdAt.Kind == DateTimeKind.Utc ? createdAt.ToLocalTime() : createdAt).Date;
    public int VacationDaysBalance { get; set; }

    public long? DepartmentId { get; set; }
    public Department Department { get; set; }

    public long? PositionId { get; set; }
    public Position Position { get; set; }

    public List<EmployeeRole> EmployeeRoles { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}