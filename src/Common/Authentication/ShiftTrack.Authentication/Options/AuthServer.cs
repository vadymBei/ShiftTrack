namespace ShiftTrack.Authentication.Options
{
    public class AuthServer
    {
        public string Authority { get; set; }

        public string Audience { get; set; }

        public bool RequireHttpsMetadata { get; set; }
    }
}
