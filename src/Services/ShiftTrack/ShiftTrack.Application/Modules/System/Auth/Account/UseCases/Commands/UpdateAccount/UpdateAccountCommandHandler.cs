using AutoMapper;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Dtos;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Application.Modules.System.Auth.Account.Dtos;
using ShiftTrack.Application.Modules.System.Auth.Account.Interfaces;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Commands.UpdateAccount;

public class UpdateAccountCommandHandler(
    IMapper mapper,
    IAccountRepository accountRepository,
    IEmployeeService employeeService,
    IEmployeeRepository employeeRepository,
    ICurrentUserService currentUserService) : IRequestHandler<UpdateAccountCommand, EmployeeVm>
{
    public async Task<EmployeeVm> Handle(UpdateAccountCommand request, CancellationToken cancellationToken = default)
    {
        var employee = await employeeRepository.GetById(request.Id, cancellationToken);

        if (employee == null
            || employee.IntegrationId != currentUserService.Employee.IntegrationId)
        {
            throw new EntityNotFoundException(typeof(Employee), request.Id);
        }

        if (employee.PhoneNumber != request.PhoneNumber
            || employee.Email != request.Email)
        {
            await accountRepository.UpdateUser(
                new UserToUpdateDto(
                    currentUserService.Employee.IntegrationId,
                    request.Email,
                    request.PhoneNumber)
                , cancellationToken);
        }

        employee = await employeeService.Update(
            new EmployeeToUpdateDto(
                request.Id,
                request.Name,
                request.Surname,
                request.Patronymic,
                request.Email,
                request.PhoneNumber,
                employee.DepartmentId,
                employee.PositionId,
                request.DateOfBirth,
                request.Gender),
            cancellationToken);

        return mapper.Map<EmployeeVm>(employee);
    }
}