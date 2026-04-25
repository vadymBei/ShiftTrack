using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Enums;

namespace ShiftTrack.Application.Modules.System.User.Common.Dtos;

public record EmployeeRoleUnitToCreateDto(
    long EmployeeRoleId,
    long UnitId,
    RoleScope Scope);