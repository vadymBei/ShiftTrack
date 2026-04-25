using ShiftTrack.Application.Modules.System.Auth.Common.Dtos;
using ShiftTrack.Application.Modules.System.Auth.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.Auth.Tokens.Commands.GenerateToken;

public record GenerateTokenCommand(
    GenerateTokenDto Data) : IRequest<TokenVm>;