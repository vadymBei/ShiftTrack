using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Employees.Common.ViewModels;
using ShiftTrack.Application.Features.Organization.Structure.Common.Interfaces;
using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Employees.Queries.GetEmployees;

public class GetEmployeesQueryHandler(
    IMapper mapper,
    IUnitService unitService,
    IDepartmentService departmentService,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<GetEmployeesQuery, IEnumerable<EmployeeVm>>
{
    public async Task<IEnumerable<EmployeeVm>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employeeQuery = applicationDbContext.Employees
            .Include(x => x.Department)
            .ThenInclude(x => x.Unit)
            .Include(x => x.Position)
            .AsQueryable();

        if (request.UnitId is not null)
        {
            await unitService.GetById(request.UnitId, cancellationToken);

            employeeQuery = employeeQuery
                .Where(x => x.Department.UnitId == request.UnitId);
        }

        if (request.DepartmentId is not null)
        {
            await departmentService.GetById(request.DepartmentId, cancellationToken);

            employeeQuery = employeeQuery
                .Where(x => x.DepartmentId == request.DepartmentId);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchPattern))
        {
            employeeQuery = employeeQuery
                .Where(x => EF.Functions.Like(
                    x.Surname.ToLower() + " " + x.Name.ToLower() + " " + x.Patronymic.ToLower(),
                    $"%{request.SearchPattern.ToLower()}%"));
        }

        var employees = await employeeQuery
            .AsNoTracking()
            .OrderBy(x => x.Surname)
            .ThenBy(x => x.Name)
            .ThenBy(x => x.Patronymic)
            .ToListAsync(cancellationToken);

        return mapper.Map<List<EmployeeVm>>(employees);
    }
}