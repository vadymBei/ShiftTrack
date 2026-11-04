using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Commands.RemovePhoto;

/// <summary>Removes the photo of the current user.</summary>
public record RemovePhotoCommand : IRequest;
