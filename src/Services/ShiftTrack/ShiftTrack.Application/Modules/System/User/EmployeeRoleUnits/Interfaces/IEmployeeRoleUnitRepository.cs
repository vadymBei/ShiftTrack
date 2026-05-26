using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Dtos;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Interfaces;

public interface IEmployeeRoleUnitRepository
{
    Task<EmployeeRoleUnit> Create(EmployeeRoleUnitToCreateDto dto, CancellationToken cancellationToken);
    Task<EmployeeRoleUnit> GetById(long id, CancellationToken cancellationToken);
    Task<IEnumerable<EmployeeRoleUnit>> GetListByEmployeeRoleId(long employeeRoleId, CancellationToken cancellationToken);
    Task<IEnumerable<EmployeeRoleUnit>> GetListByUnitId(long unitId, CancellationToken cancellationToken);
    Task CheckIfExists(EmployeeRoleUnitToCreateDto dto, CancellationToken cancellationToken);
    Task RecalculateScope(long employeeRoleUnitId, CancellationToken cancellationToken);
    Task Delete(long id, CancellationToken cancellationToken);
    
}