using AutoMapper;
using ShiftTrack.Domain.Modules.System.Auth.Models;

namespace ShiftTrack.Application.Modules.System.Auth.Tokens.ViewModels;

[AutoMap(typeof(Token))]
public class TokenVm
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string TokenType { get; set; }
    public int ExpiresIn { get; set; }
}