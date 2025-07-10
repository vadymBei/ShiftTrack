namespace ShiftTrack.Application.Features.System.User.Common.Dtos;

public record EmployeeRoleUnitDepartmentsToCreateDto(
    long EmployeeRoleUnitId,
    IEnumerable<long> DepartmentIds);