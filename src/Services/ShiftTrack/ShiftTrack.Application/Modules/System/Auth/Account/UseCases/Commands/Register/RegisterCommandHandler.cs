using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Employees.Dtos;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Application.Modules.System.Auth.Account.Dtos;
using ShiftTrack.Application.Modules.System.Auth.Account.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Commands.Register;

public class RegisterCommandHandler(
    IMapper mapper,
    IAccountRepository accountRepository,
    IEmployeeRepository employeeRepository)
    : IRequestHandler<RegisterCommand, EmployeeVm>
{
    public async Task<EmployeeVm> Handle(RegisterCommand request, CancellationToken cancellationToken = default)
    {
        var employee = await employeeRepository.GetByPhoneNumber(request.PhoneNumber, cancellationToken);

        if (employee != null)
        {
            throw new UserAlreadyExistException(request.PhoneNumber);
        }

        var user = await accountRepository.RegisterUser(
            new UserToRegisterDto(
                request.PhoneNumber,
                request.Email,
                request.Password),
            cancellationToken);

        employee = await employeeRepository.Create(
            new EmployeeToCreateDto(
                user.Id,
                request.Name,
                request.Surname,
                request.Patronymic,
                request.Email,
                request.PhoneNumber,
                request.Gender),
            cancellationToken);

        return mapper.Map<EmployeeVm>(employee);
    }
}