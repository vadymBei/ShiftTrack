using AutoMapper;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;
using ShiftTrack.Domain.Features.System.User.Employees.Enums;

namespace ShiftTrack.Application.Features.Organization.Employees.Common.ViewModels;

[AutoMap(typeof(Employee))]
public class EmployeeVm
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    
    public long? DepartmentId { get; set; }
    public DepartmentVm Department { get; set; }

    public long? PositionId { get; set; }
    public PositionVm Position { get; set; }

    public EmployeeGender Gender { get; set; }
}