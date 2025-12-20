namespace ShiftTrack.Application.Features.Organization.Employees.Common.Dtos;

public record EmployeesFilterDto(
    string SearchPattern,
    long? DepartmentId);