namespace ShiftTrack.Application.Features.System.User.Common.Dtos;

public record UserToUpdateDto(
    string Id,
    string Email,
    string PhoneNumber);