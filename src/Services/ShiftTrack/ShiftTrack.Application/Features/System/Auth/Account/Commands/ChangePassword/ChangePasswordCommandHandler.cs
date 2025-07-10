using AutoMapper;
using ShiftTrack.Application.Features.Organization.Employees.Common.Interfaces;
using ShiftTrack.Application.Features.System.Auth.Common.ViewModels;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.Auth.Account.Commands.ChangePassword;

public class ChangePasswordCommandHandler(
    IMapper mapper,
    IEmployeeService employeeService) : IRequestHandler<ChangePasswordCommand, TokenVm>
{
    public async Task<TokenVm> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var token = await employeeService
            .ChangePassword(request.Data, cancellationToken);

        return mapper.Map<TokenVm>(token);
    }
}