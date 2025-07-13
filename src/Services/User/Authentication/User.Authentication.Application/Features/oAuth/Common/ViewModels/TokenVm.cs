using AutoMapper;
using User.Authentication.Domain.Features.oAuth.Models;

namespace User.Authentication.Application.Features.oAuth.Common.ViewModels;

[AutoMap(typeof(Token))]
public class TokenVm
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string TokenType { get; set; }
    public int ExpiresIn { get; set; }
}