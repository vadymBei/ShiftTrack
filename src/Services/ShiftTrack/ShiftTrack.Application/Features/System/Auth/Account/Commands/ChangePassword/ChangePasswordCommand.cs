using ShiftTrack.Application.Features.System.Auth.Common.Dtos;
using ShiftTrack.Application.Features.System.Auth.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.Auth.Account.Commands.ChangePassword;

public record ChangePasswordCommand(
    ChangePasswordDto Data) : IRequest<TokenVm>;