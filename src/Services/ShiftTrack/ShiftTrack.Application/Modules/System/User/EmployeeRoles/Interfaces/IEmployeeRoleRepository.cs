using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Dtos;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;

public interface IEmployeeRoleRepository
{
    Task<EmployeeRole> Create(EmployeeRoleToCreateDto dto, CancellationToken cancellationToken);
    Task CheckIfExists(EmployeeRoleToCreateDto dto, CancellationToken cancellationToken);
    Task<EmployeeRole> GetById(long id, CancellationToken cancellationToken);
    Task<IEnumerable<EmployeeRole>> GetListByEmployeeId(long employeeId, CancellationToken cancellationToken);
    Task RecalculateScope(long id, CancellationToken cancellationToken);
    Task Delete(long id, CancellationToken cancellationToken);
}