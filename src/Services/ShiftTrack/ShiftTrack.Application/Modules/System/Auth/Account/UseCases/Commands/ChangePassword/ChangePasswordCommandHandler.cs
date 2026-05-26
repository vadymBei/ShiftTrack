using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.System.Auth.Account.Dtos;
using ShiftTrack.Application.Modules.System.Auth.Account.Interfaces;
using ShiftTrack.Application.Modules.System.Auth.Tokens.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Commands.ChangePassword;

public class ChangePasswordCommandHandler(
    IMapper mapper,
    IAccountRepository accountRepository,
    IEmployeeRepository employeeRepository) : IRequestHandler<ChangePasswordCommand, TokenVm>
{
    public async Task<TokenVm> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetById(request.Data.EmployeeId, cancellationToken);

        var token = await accountRepository.ChangePassword(
            new ChangeUserPasswordDto(
                employee.IntegrationId,
                request.Data.OldPassword,
                request.Data.NewPassword,
                request.Data.ConfirmPassword),
            cancellationToken);

        return mapper.Map<TokenVm>(token);
    }
}