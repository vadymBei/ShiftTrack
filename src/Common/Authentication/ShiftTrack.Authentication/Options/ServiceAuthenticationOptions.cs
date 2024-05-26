namespace ShiftTrack.Authentication.Options
{
    public class ServiceAuthenticationOptions
    {
        public AuthServer AuthServer { get; set; }

        public List<ServiceUser> Users { get; set; }
    }
}
