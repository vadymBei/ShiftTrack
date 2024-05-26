using ShiftTrack.Authentication.Models;

namespace ShiftTrack.Authentication.Interfaces
{
    public interface ICurrentUserService
    {
        public User User { get; }
    }
}
