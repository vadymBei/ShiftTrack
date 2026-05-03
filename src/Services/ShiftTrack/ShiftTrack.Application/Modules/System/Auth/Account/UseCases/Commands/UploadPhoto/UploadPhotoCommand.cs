using Microsoft.AspNetCore.Http;
using ShiftTrack.Application.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Commands.UploadPhoto;

public record UploadPhotoCommand(
    long EmployeeId,
    IFormFile File) : IRequest<DocumentVm>;