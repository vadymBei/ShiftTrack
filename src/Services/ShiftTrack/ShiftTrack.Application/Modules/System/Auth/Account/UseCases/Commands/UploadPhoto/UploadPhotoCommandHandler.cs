using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Constants;
using ShiftTrack.Application.Modules.Organization.Employees.Dtos;
using ShiftTrack.Application.Modules.Organization.Employees.Exceptions;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Models;
using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Commands.UploadPhoto;

public class UploadPhotoCommandHandler(
    ICurrentUserService currentUserService,
    IEmployeeRepository employeeRepository,
    IEmployeePhotoStorage photoStorage,
    IUnitOfWork unitOfWork) : IRequestHandler<UploadPhotoCommand, EmployeePhotoVm>
{
    public async Task<EmployeePhotoVm> Handle(UploadPhotoCommand request, CancellationToken cancellationToken = default)
    {
        var employeeId = currentUserService.Employee.Id;

        if (request.File.Length > EmployeePhotoConstants.MaxSize)
            throw new EmployeeException(EmployeeExceptionsLocalization.PHOTO_TOO_LARGE,
                nameof(EmployeeExceptionsLocalization.PHOTO_TOO_LARGE));

        using var content = new MemoryStream();
        await request.File.Content.CopyToAsync(content, cancellationToken);

        if (content.Length == 0)
            throw new EmployeeException(EmployeeExceptionsLocalization.PHOTO_REQUIRED,
                nameof(EmployeeExceptionsLocalization.PHOTO_REQUIRED));

        if (content.Length > EmployeePhotoConstants.MaxSize)
            throw new EmployeeException(EmployeeExceptionsLocalization.PHOTO_TOO_LARGE,
                nameof(EmployeeExceptionsLocalization.PHOTO_TOO_LARGE));

        var format = PhotoFormat.Detect(content.GetBuffer().AsSpan(0, (int)Math.Min(content.Length, 12)))
                     ?? throw new EmployeeException(EmployeeExceptionsLocalization.PHOTO_TYPE_NOT_ALLOWED,
                         nameof(EmployeeExceptionsLocalization.PHOTO_TYPE_NOT_ALLOWED));

        var previous = (await employeeRepository.GetPhotoNames([employeeId], cancellationToken))
            .GetValueOrDefault(employeeId);

        content.Position = 0;
        var storedName = await photoStorage.Save(content, format.Extension, cancellationToken);

        try
        {
            await employeeRepository.UpdatePhoto(new UploadEmployeePhotoDto(employeeId, storedName), cancellationToken);
        }
        catch
        {
            photoStorage.Delete(storedName);
            throw;
        }

        if (!string.IsNullOrEmpty(previous))
            await unitOfWork.AfterCommit(() =>
            {
                photoStorage.Delete(previous);
                return Task.CompletedTask;
            });

        return new EmployeePhotoVm { EmployeeId = employeeId, Version = PhotoFormat.VersionOf(storedName) };
    }
}
