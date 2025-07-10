namespace ShiftTrack.Application.Features.System.User.Common.Dtos;

public record EmployeeRoleToCreateDto(
    long EmployeeId,
    long RoleId,
    long? UnitId,
    IEnumerable<long> DepartmentIds);