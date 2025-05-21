using ShiftTrack.Core.Application.System.Auth.Common.ViewModels;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.Employees.Commands.ChangePassword;

public record ChangePasswordCommand(
    ChangeEmployeePasswordDto Data) : IRequest<TokenVM>;