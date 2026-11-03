using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Employees.Dtos;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Employees.UseCases.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler(
    IMapper mapper,
    IEmployeeService employeeService,
    IEmployeeRepository employeeRepository,
    IVacationAccrualService vacationAccrualService,
    IEmployeeEditPermission employeeEditPermission)
    : IRequestHandler<UpdateEmployeeCommand, EmployeeVm>
{
    public async Task<EmployeeVm> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetById(request.Id, cancellationToken);
        var serviceStart = employee.ServiceStart;

        await employeeEditPermission.EnsureCanEdit(employee, request.DepartmentId, cancellationToken);

        employee = await employeeService.Update(
            new EmployeeToUpdateDto(
                request.Id,
                request.Name,
                request.Surname,
                request.Patronymic,
                employee.Email,
                employee.PhoneNumber,
                request.DepartmentId,
                request.PositionId,
                request.DateOfBirth,
                request.Gender,
                request.HireDate),
            cancellationToken);

        // The accrued days and the balance depend on the length of service: recount them now, not at night.
        if (employee.ServiceStart != serviceStart)
            await vacationAccrualService.Recalculate(employee.Id, cancellationToken);

        return mapper.Map<EmployeeVm>(employee);
    }
}