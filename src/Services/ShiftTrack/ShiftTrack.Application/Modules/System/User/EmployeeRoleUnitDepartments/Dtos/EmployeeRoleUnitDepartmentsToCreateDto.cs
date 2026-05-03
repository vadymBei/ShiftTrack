namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Dtos;

public record EmployeeRoleUnitDepartmentsToCreateDto(
    long EmployeeRoleUnitId,
    IEnumerable<long> DepartmentIds);