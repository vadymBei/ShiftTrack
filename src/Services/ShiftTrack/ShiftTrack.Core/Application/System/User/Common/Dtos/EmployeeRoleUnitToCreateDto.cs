using ShiftTrack.Core.Domain.System.User.EmployeeRoles.Enums;

namespace ShiftTrack.Core.Application.System.User.Common.Dtos;

public record EmployeeRoleUnitToCreateDto(
    long EmployeeRoleId,
    long UnitId,
    RoleScope Scope);