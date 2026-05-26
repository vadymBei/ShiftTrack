namespace ShiftTrack.Application.Modules.System.User.EmployeeRoles.Dtos;

public record EmployeeRoleToCreateDto(
    long EmployeeId,
    long RoleId,
    long? UnitId,
    IEnumerable<long> DepartmentIds);