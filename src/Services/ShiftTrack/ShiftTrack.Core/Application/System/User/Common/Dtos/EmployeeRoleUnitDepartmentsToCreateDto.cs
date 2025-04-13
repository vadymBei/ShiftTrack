using ShiftTrack.Core.Domain.Organization.Structure.Entities;

namespace ShiftTrack.Core.Application.System.User.Common.Dtos;

public record EmployeeRoleUnitDepartmentsToCreateDto(
    long EmployeeRoleUnitId,
    IEnumerable<Department> Departments);