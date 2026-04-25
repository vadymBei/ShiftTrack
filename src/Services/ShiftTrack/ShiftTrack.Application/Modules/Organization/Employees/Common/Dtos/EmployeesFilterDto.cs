namespace ShiftTrack.Application.Modules.Organization.Employees.Common.Dtos;

public record EmployeesFilterDto(
    string SearchPattern,
    long? UnitId,
    long? DepartmentId);