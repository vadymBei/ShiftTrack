namespace ShiftTrack.Application.Modules.System.Auth.Common.Dtos;

public record GenerateTokenDto(
    string Login,
    string Password);