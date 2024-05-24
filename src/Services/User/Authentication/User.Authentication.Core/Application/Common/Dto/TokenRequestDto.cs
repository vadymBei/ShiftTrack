namespace User.Authentication.Core.Application.Common.Dto
{
    public class TokenRequestDto()
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Client { get; set; }
        public string Scope { get; set; }
        public string ClientSecret { get; set; }
    };
}
