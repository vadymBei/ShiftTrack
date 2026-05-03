using ShiftTrack.Domain.Modules.System.User.Employees.Enums;

namespace ShiftTrack.Application.Modules.Organization.Employees.Dtos;

public record EmployeeToCreateDto(
    string IntegrationId,
    string Name,
    string Surname,
    string Patronymic,
    string Email,
    string PhoneNumber,
    EmployeeGender Gender);