using AutoMapper;

namespace User.Authentication.Core.Application.Common.ViewModels
{
    [AutoMap(typeof(ShiftTrack.Authentication.Models.User))]
    public class UserVM
    {
        public string Login { get; set; }

        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
