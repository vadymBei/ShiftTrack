using ShiftTrack.Data.Interfaces;
using ShiftTrack.Domain.Features.Organization.Structure.Entities;

namespace ShiftTrack.Application.Features.Organization.Structure.Common.Interfaces;

public interface IDepartmentService : IEntityServiceBase<Department>
{
    Task<IEnumerable<Department>> GetDepartmentsByIds(IEnumerable<long> departmentIds, CancellationToken cancellationToken);
}