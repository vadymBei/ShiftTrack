using AutoMapper;
using ShiftTrack.Core.Domain.System.Tokens.Models;

namespace ShiftTrack.Core.Application.System.Auth.Common.ViewModels
{
    [AutoMap(typeof(Token))]
    public class TokenVM
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public string TokenType { get; set; }

        public int ExpiresIn { get; set; }
    }
}
