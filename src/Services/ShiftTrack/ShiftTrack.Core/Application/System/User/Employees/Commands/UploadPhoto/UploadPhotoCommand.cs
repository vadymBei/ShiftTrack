using Microsoft.AspNetCore.Http;
using ShiftTrack.Core.Application.Data.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.Employees.Commands.UploadPhoto;

public record UploadPhotoCommand(
    long EmployeeId,
    IFormFile File) : IRequest<DocumentVm>;