namespace ShiftTrack.Core.Application.System.User.Common.Dtos
{
    public record UserToUpdateDto(
        string Id,
        string Email,
        string PhoneNumber);
}
