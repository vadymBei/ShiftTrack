namespace ShiftTrack.Application.Modules.System.User.Common.Dtos;

public record EmployeeRoleToCreateDto(
    long EmployeeId,
    long RoleId,
    long? UnitId,
    IEnumerable<long> DepartmentIds);