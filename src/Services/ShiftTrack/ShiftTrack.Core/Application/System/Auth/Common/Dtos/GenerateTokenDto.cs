namespace ShiftTrack.Core.Application.System.Auth.Common.Dtos
{
    public record GenerateTokenDto(
        string Login,
        string Password);
}
