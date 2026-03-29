using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Common.ViewModels;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.System.Auth.Account.Queries.GetUserPhoto;

public class GetUserPhotoQueryHandler(
    IHostEnvironment hostEnvironment,
    IApplicationDbContext applicationDbContext) : IRequestHandler<GetUserPhotoQuery, DocumentVm>
{
    public async Task<DocumentVm> Handle(GetUserPhotoQuery request, CancellationToken cancellationToken = default)
    {
        var employee = await applicationDbContext.Employees
                           .FirstOrDefaultAsync(x => x.Id == request.EmployeeId, cancellationToken)
                       ?? throw new EntityNotFoundException(typeof(Employee), request.EmployeeId);

        if (string.IsNullOrEmpty(employee.PhotoFullName))
        {
            employee.PhotoFullName = "no-photo.png";
        }

        var folderPath = Path.Combine(
            hostEnvironment.ContentRootPath,
            "wwwroot",
            "uploads",
            "employees");

        var photoPath = Path.Combine(folderPath, employee.PhotoFullName);

        if (!File.Exists(photoPath))
        {
            photoPath = Path.Combine(folderPath, "no-photo.png");

            if (!File.Exists(photoPath))
            {
                throw new FileNotFoundException($"Photo {employee.PhotoFullName} not found.");
            }
        }

        return new DocumentVm()
        {
            Name = employee.PhotoFullName,
            Path = photoPath,
            Extension = Path.GetExtension(employee.PhotoFullName)
        };
    }
}