using ShiftTrack.Domain.Modules.System.User.Employees.Enums;

namespace ShiftTrack.Application.Modules.Organization.Employees.Dtos;

public record EmployeeToUpdateDto(
    long Id,
    string Name,
    string Surname,
    string Patronymic,
    string Email,
    string PhoneNumber,
    long? DepartmentId,
    long? PositionId,
    DateTime? DateOfBirth,
    EmployeeGender Gender);