using Microsoft.AspNetCore.Http;
using ShiftTrack.Application.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.Auth.Account.Commands.UploadPhoto;

public record UploadPhotoCommand(
    long EmployeeId,
    IFormFile File) : IRequest<DocumentVm>;