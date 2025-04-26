namespace ShiftTrack.Core.Application.System.User.Common.Dtos;

public record EmployeeRoleUnitDepartmentsToCreateDto(
    long EmployeeRoleUnitId,
    IEnumerable<long> DepartmentIds);