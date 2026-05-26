using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.System.User.Common.Constants;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Exceptions;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Dtos;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Interfaces;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;
using ShiftTrack.Infrastructure.Common.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Infrastructure.Modules.System.User.EmployeeRoleUnitDepartments.Repositories;

public class EmployeeRoleUnitDepartmentRepository(
    IApplicationDbContext applicationDbContext) : IEmployeeRoleUnitDepartmentRepository
{
    public async Task<IEnumerable<EmployeeRoleUnitDepartment>> Create(EmployeeRoleUnitDepartmentsToCreateDto dto,
        CancellationToken cancellationToken)
    {
        var employeeRoleUnitDepartments = dto.DepartmentIds
            .Select(x =>
                new EmployeeRoleUnitDepartment()
                {
                    EmployeeRoleUnitId = dto.EmployeeRoleUnitId,
                    DepartmentId = x
                })
            .ToList();

        applicationDbContext.EmployeeRoleUnitDepartments.AddRange(employeeRoleUnitDepartments);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return employeeRoleUnitDepartments;
    }

    public async Task CheckIfExists(EmployeeRoleUnitDepartmentsToCreateDto dto, CancellationToken cancellationToken)
    {
        var isAlreadyEmployeeRoleUnitDepartmentExists = await applicationDbContext.EmployeeRoleUnitDepartments
            .AnyAsync(x => x.EmployeeRoleUnitId == dto.EmployeeRoleUnitId
                           && dto.DepartmentIds.Contains((long)x.DepartmentId),
                cancellationToken);

        if (isAlreadyEmployeeRoleUnitDepartmentExists)
        {
            throw new EmployeeRoleAlreadyExistException(
                UserExceptionsLocalization.EMPLOYEE_ROLE_UNIT_DEPARTMENT_ALREADY_EXISTS,
                nameof(UserExceptionsLocalization.EMPLOYEE_ROLE_UNIT_DEPARTMENT_ALREADY_EXISTS));
        }
    }

    public async Task<IEnumerable<EmployeeRoleUnitDepartment>> GetListByEmployeeRoleUnitId(
        long employeeRoleUnitId,
        CancellationToken cancellationToken)
    {
        var employeeRoleUnitDepartments = await applicationDbContext.EmployeeRoleUnitDepartments
            .AsNoTracking()
            .Include(x => x.Department)
            .Where(x => x.EmployeeRoleUnitId == employeeRoleUnitId)
            .ToListAsync(cancellationToken);

        return employeeRoleUnitDepartments;
    }

    public async Task<EmployeeRoleUnitDepartment> GetById(long id, CancellationToken cancellationToken)
    {
        var employeeRoleUnitDepartment = await applicationDbContext.EmployeeRoleUnitDepartments
                                             .Include(x => x.EmployeeRoleUnit)
                                             .ThenInclude(x => x.EmployeeRole)
                                             .ThenInclude(x => x.Employee)
                                             .ThenInclude(x => x.Department)
                                             .Include(x => x.EmployeeRoleUnit)
                                             .ThenInclude(x => x.EmployeeRole)
                                             .ThenInclude(x => x.Role)
                                             .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                                         ?? throw new EntityNotFoundException(typeof(EmployeeRoleUnitDepartment), id);

        return employeeRoleUnitDepartment;
    }

    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        var employeeRoleUnitDepartment = await applicationDbContext.EmployeeRoleUnitDepartments
                                             .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                                         ?? throw new EntityNotFoundException(typeof(EmployeeRoleUnitDepartment), id);

        applicationDbContext.EmployeeRoleUnitDepartments.Remove(employeeRoleUnitDepartment);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}