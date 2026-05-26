using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Employees.Dtos;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Employees.UseCases.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler(
    IMapper mapper,
    IEmployeeService employeeService,
    IEmployeeRepository employeeRepository)
    : IRequestHandler<UpdateEmployeeCommand, EmployeeVm>
{
    public async Task<EmployeeVm> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetById(request.Id, cancellationToken);
        
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
                request.Gender),
            cancellationToken);

        return mapper.Map<EmployeeVm>(employee);
    }
}