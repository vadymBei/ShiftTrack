using MediatR;
using User.Authentication.Core.Application.Common.Dto;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Users.Commands.ChangePassword
{
    public record ChangePasswordCommand(
        ChangePasswordDto Data) : IRequest<TokenVM>;
}
