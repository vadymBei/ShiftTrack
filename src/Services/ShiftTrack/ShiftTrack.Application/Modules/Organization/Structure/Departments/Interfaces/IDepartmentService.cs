using ShiftTrack.Application.Modules.Organization.Structure.Departments.Models;
using ShiftTrack.Domain.Modules.Organization.Structure.Entities;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<GroupedDepartmentsByUnit>> GetGroupedDepartmentsByUnit(CancellationToken cancellationToken);
    Task<IEnumerable<Department>> GetDepartmentsByRoles(long unitId, CancellationToken cancellationToken);
}