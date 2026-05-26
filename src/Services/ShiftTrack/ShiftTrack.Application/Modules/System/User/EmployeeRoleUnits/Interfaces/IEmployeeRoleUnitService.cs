using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Dtos;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Interfaces;

public interface IEmployeeRoleUnitService
{
    Task<EmployeeRoleUnit> Create(EmployeeRoleUnitToCreateDto dto, CancellationToken cancellationToken);
    Task Delete(long employeeRoleUnitId, CancellationToken cancellationToken);
}