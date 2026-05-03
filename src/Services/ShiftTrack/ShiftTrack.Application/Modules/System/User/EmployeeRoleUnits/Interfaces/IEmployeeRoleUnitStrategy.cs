using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Dtos;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Interfaces;

public interface IEmployeeRoleUnitStrategy
{
    Task<EmployeeRoleUnit> Create(EmployeeRoleUnitToCreateDto dto, CancellationToken cancellationToken);
    Task Delete(long employeeUnitId, CancellationToken cancellationToken);
}