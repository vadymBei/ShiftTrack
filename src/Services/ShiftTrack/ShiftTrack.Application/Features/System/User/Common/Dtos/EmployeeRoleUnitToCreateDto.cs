using ShiftTrack.Domain.Features.System.User.EmployeeRoles.Enums;

namespace ShiftTrack.Application.Features.System.User.Common.Dtos;

public record EmployeeRoleUnitToCreateDto(
    long EmployeeRoleId,
    long UnitId,
    RoleScope Scope);