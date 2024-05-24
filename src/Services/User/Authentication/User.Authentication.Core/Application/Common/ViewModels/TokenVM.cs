using AutoMapper;
using User.Authentication.Core.Domain.Models.OAuth;

namespace User.Authentication.Core.Application.Common.ViewModels
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
