using ShiftTrack.Application.Modules.System.Auth.Account.Dtos;
using ShiftTrack.Application.Modules.System.Auth.Tokens.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Commands.ChangePassword;

public record ChangePasswordCommand(
    ChangePasswordDto Data) : IRequest<TokenVm>;