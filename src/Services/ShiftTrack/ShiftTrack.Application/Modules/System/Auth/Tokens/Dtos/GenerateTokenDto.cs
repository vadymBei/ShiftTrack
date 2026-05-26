namespace ShiftTrack.Application.Modules.System.Auth.Tokens.Dtos;

public record GenerateTokenDto(
    string Login,
    string Password);