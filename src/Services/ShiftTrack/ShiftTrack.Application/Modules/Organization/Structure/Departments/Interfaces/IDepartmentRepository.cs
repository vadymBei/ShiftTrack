using ShiftTrack.Application.Modules.Organization.Structure.Departments.Dtos;
using ShiftTrack.Domain.Modules.Organization.Structure.Entities;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;

public interface IDepartmentRepository
{
    Task<Department> Create(DepartmentToCreateDto departmentToCreateDto, CancellationToken cancellationToken);
    Task<Department> Update(DepartmentToUpdateDto departmentToUpdateDto, CancellationToken cancellationToken);
    Task Delete(long id, CancellationToken cancellationToken);
    Task DeleteByUnitId(long id, CancellationToken cancellationToken);
    Task<Department> GetById(long id, CancellationToken cancellationToken);
    Task<IEnumerable<Department>> GetByIds(IEnumerable<long> ids, CancellationToken cancellationToken);
    Task<IEnumerable<Department>> GetAll(CancellationToken cancellationToken);
    Task<IEnumerable<Department>> GetDepartmentsByIds(IEnumerable<long> departmentIds, CancellationToken cancellationToken);
    Task<IEnumerable<Department>> GetDepartmentsByUnitId(long unitId, CancellationToken cancellationToken);
}