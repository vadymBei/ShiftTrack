using ShiftTrack.Data.Interfaces;
using ShiftTrack.Domain.Modules.Organization.Structure.Entities;

namespace ShiftTrack.Application.Modules.Organization.Structure.Common.Interfaces;

public interface IDepartmentService : IEntityServiceBase<Department>
{
    Task<IEnumerable<Department>> GetDepartmentsByIds(IEnumerable<long> departmentIds, CancellationToken cancellationToken);
}