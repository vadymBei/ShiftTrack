namespace ShiftTrack.Application.Features.System.Auth.Common.Dtos;

public record GenerateTokenDto(
    string Login,
    string Password);