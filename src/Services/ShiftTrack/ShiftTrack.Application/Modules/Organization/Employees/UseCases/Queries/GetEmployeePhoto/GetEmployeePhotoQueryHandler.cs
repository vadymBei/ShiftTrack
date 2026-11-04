using System.Net;
using ShiftTrack.Application.Modules.Organization.Employees.Constants;
using ShiftTrack.Application.Modules.Organization.Employees.Exceptions;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Models;
using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Employees.UseCases.Queries.GetEmployeePhoto;

public class GetEmployeePhotoQueryHandler(
    IEmployeeRepository employeeRepository,
    IEmployeePhotoStorage photoStorage) : IRequestHandler<GetEmployeePhotoQuery, EmployeePhotoFileVm>
{
    public async Task<EmployeePhotoFileVm> Handle(GetEmployeePhotoQuery request,
        CancellationToken cancellationToken = default)
    {
        var photos = await employeeRepository.GetPhotoNames([request.EmployeeId], cancellationToken);
        var content = photos.TryGetValue(request.EmployeeId, out var storedName) ? photoStorage.Open(storedName) : null;

        if (content is null)
            throw new EmployeeException(
                EmployeeExceptionsLocalization.PHOTO_NOT_FOUND,
                nameof(EmployeeExceptionsLocalization.PHOTO_NOT_FOUND),
                HttpStatusCode.NotFound);

        return new EmployeePhotoFileVm { Content = content, ContentType = PhotoFormat.ContentTypeOf(storedName) };
    }
}
