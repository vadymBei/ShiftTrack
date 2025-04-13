using ShiftTrack.Core.Domain.Organization.Structure.Entities;
using ShiftTrack.Data.Interfaces;

namespace ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;

public interface IDepartmentService : IEntityServiceBase<Department>
{
    Task<IEnumerable<Department>> GetDepartmentsByIds(IEnumerable<long> departmentIds, CancellationToken cancellationToken);
}