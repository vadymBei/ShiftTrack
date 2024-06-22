using AutoMapper;
using User.Authentication.Core.Domain.Models.OAuth;

namespace User.Authentication.Core.Application.Common.ViewModels
{
    [AutoMap(typeof(Token))]
    public record TokenVM(
        string AccessToken,
        string RefreshToken,
        string TokenType,
        int ExpiresIn);
}
