using MediatR;
using ShiftTrack.Core.Application.System.Auth.Common.ViewModels;
using ShiftTrack.Core.Application.System.User.Common.Dtos;

namespace ShiftTrack.Core.Application.System.User.Employees.Commands.ChangePassword
{
    public record ChangePasswordCommand(
        ChangeEmployeePasswordDto Data) : IRequest<TokenVM>;
}
