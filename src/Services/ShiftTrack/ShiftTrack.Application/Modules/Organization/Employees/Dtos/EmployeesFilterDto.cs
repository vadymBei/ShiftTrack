namespace ShiftTrack.Application.Modules.Organization.Employees.Dtos;

public record EmployeesFilterDto(
    string SearchPattern,
    long? UnitId,
    long? DepartmentId);