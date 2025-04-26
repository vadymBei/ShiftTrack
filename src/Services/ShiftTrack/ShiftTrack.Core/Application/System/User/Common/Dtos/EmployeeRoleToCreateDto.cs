namespace ShiftTrack.Core.Application.System.User.Common.Dtos;

public record EmployeeRoleToCreateDto(
    long EmployeeId,
    long RoleId,
    long? UnitId,
    IEnumerable<long> DepartmentIds);