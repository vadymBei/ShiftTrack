using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Employees.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Employees.Common.ViewModels;
using ShiftTrack.Application.Features.Organization.Structure.Common.Interfaces;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.Organization.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler(
    IMapper mapper,
    IPositionService positionService,
    IEmployeeService employeeService,
    IDepartmentService departmentService,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<UpdateEmployeeCommand, EmployeeVm>
{
    public async Task<EmployeeVm> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await applicationDbContext.Employees
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (employee == null)
        {
            throw new EntityNotFoundException(typeof(Employee), request.Id);
        }

        employee.Name = request.Name;
        employee.Surname = request.Surname;
        employee.Patronymic = request.Patronymic;
        employee.Email = request.Email;
        employee.DateOfBirth = request.DateOfBirth;
        employee.Gender = request.Gender;

        if (request.DepartmentId is not null)
        {
            await departmentService
                .GetById(request.DepartmentId, cancellationToken);

            employee.DepartmentId = request.DepartmentId;
        }
        else
        {
            employee.DepartmentId = null;
        }

        if (request.PositionId is not null)
        {
            await positionService
                .GetById(request.PositionId, cancellationToken);

            employee.PositionId = request.PositionId;
        }
        else
        {
            employee.PositionId = null;
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        employee = await employeeService
            .GetById(request.Id, cancellationToken);

        return mapper.Map<EmployeeVm>(employee);
    }
}