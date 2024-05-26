namespace ShiftTrack.Authentication.Options
{
    public class ServiceUser
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string[] Roles { get; set; }
    }
}
