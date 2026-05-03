using ShiftTrack.Application.Modules.System.Auth.Tokens.Dtos;
using ShiftTrack.Application.Modules.System.Auth.Tokens.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.Auth.Tokens.UseCases.Commands.GenerateToken;

public record GenerateTokenCommand(
    GenerateTokenDto Data) : IRequest<TokenVm>;