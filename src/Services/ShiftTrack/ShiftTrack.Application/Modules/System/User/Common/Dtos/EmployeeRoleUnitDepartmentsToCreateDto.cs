namespace ShiftTrack.Application.Modules.System.User.Common.Dtos;

public record EmployeeRoleUnitDepartmentsToCreateDto(
    long EmployeeRoleUnitId,
    IEnumerable<long> DepartmentIds);