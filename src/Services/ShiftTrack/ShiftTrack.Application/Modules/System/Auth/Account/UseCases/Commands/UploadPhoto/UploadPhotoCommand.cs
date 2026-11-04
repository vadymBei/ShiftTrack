using ShiftTrack.Application.Modules.Organization.Employees.Models;
using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Commands.UploadPhoto;

/// <summary>Replaces the photo of the current user.</summary>
public record UploadPhotoCommand(
    PhotoUpload File) : IRequest<EmployeePhotoVm>;
