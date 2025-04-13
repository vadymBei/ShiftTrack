using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Organization.Structure.Common.Services;

public class DepartmentService(
    IApplicationDbContext dbContext) : IDepartmentService
{
    public async Task<Department> GetById(object id, CancellationToken cancellationToken)
    {
        var departmentId = (long)id;

        var department = await dbContext.Departments
            .AsNoTracking()
            .Include(x => x.Unit)
            .FirstOrDefaultAsync(x => x.Id == departmentId, cancellationToken);

        if (department == null)
            throw new EntityNotFoundException(typeof(Department), departmentId);

        return department;
    }

    public async Task<IEnumerable<Department>> GetDepartmentsByIds(IEnumerable<long> departmentIds, CancellationToken cancellationToken)
    {
        var departments = await dbContext.Departments
            .AsNoTracking()
            .Include(x => x.Unit)
            .Where(x => departmentIds.Contains(x.Id))
            .ToListAsync(cancellationToken);
        
        return departments;
    }
}