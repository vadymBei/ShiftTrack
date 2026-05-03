using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Domain.Modules.Organization.Structure.Entities;
using ShiftTrack.Infrastructure.Common.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Infrastructure.Modules.Organization.Structure.Departments.Repositories;

public class DepartmentRepository(
    IApplicationDbContext applicationDbContext) : IDepartmentRepository
{
    public async Task DeleteByUnitId(long id, CancellationToken cancellationToken)
    {
        var departments = await applicationDbContext.Departments
            .Where(x => x.UnitId == id)
            .ToListAsync(cancellationToken);

        applicationDbContext.Departments.RemoveRange(departments);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Department> GetById(long id, CancellationToken cancellationToken)
    {
        var department = await applicationDbContext.Departments
                             .AsNoTracking()
                             .Include(x => x.Unit)
                             .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                         ?? throw new EntityNotFoundException(typeof(Department), id);

        return department;
    }

    public async Task<IEnumerable<Department>> GetByIds(IEnumerable<long> ids, CancellationToken cancellationToken)
    {
        return await applicationDbContext.Departments
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Department>> GetAll(CancellationToken cancellationToken)
    {
        return await applicationDbContext.Departments
            .AsNoTracking()
            .Include(x => x.Unit)
            .ToListAsync(cancellationToken);
    }

    public async Task<Department> Create(DepartmentToCreateDto departmentToCreateDto,
        CancellationToken cancellationToken)
    {
        var department = new Department()
        {
            Name = departmentToCreateDto.Name,
            UnitId = departmentToCreateDto.UnitId
        };

        await applicationDbContext.Departments.AddAsync(department, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return department;
    }

    public async Task<Department> Update(DepartmentToUpdateDto departmentToUpdateDto,
        CancellationToken cancellationToken)
    {
        var department = await applicationDbContext.Departments
                             .FirstOrDefaultAsync(x => x.Id == departmentToUpdateDto.Id, cancellationToken)
                         ?? throw new EntityNotFoundException(typeof(Department), departmentToUpdateDto.Id);

        department.Name = departmentToUpdateDto.Name;

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return department;
    }

    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        var department = await applicationDbContext.Departments
                             .FindAsync([id], cancellationToken)
                         ?? throw new EntityNotFoundException(typeof(Department), id);

        applicationDbContext.Departments.Remove(department);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Department>> GetDepartmentsByIds(IEnumerable<long> departmentIds,
        CancellationToken cancellationToken)
    {
        var departments = await applicationDbContext.Departments
            .AsNoTracking()
            .Include(x => x.Unit)
            .Where(x => departmentIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        return departments;
    }

    public async Task<IEnumerable<Department>> GetDepartmentsByUnitId(long unitId, CancellationToken cancellationToken)
    {
        return await applicationDbContext.Departments
            .Include(x => x.Unit)
            .Where(x => x.UnitId == unitId)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }
}