using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Structure.Common.Interfaces;
using ShiftTrack.Domain.Features.Organization.Structure.Entities;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.Organization.Structure.Common.Services;

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