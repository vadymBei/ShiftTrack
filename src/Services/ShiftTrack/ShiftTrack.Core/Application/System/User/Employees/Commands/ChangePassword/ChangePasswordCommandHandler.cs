using AutoMapper;
using ShiftTrack.Core.Application.System.Auth.Common.ViewModels;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.Employees.Commands.ChangePassword;

public class ChangePasswordCommandHandler(
    IMapper mapper,
    IEmployeeService employeeService) : IRequestHandler<ChangePasswordCommand, TokenVM>
{
    public async Task<TokenVM> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var token = await employeeService
            .ChangePassword(request.Data, cancellationToken);

        return mapper.Map<TokenVM>(token);
    }
}