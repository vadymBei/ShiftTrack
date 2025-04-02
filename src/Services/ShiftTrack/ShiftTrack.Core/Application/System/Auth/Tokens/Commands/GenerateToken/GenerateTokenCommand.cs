using MediatR;
using ShiftTrack.Core.Application.System.Auth.Common.Dtos;
using ShiftTrack.Core.Application.System.Auth.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.Auth.Tokens.Commands.GenerateToken;

public record GenerateTokenCommand(
    GenerateTokenDto Data) : IRequest<TokenVM>;