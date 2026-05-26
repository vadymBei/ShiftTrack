using Microsoft.Extensions.Hosting;
using ShiftTrack.Application.Common.ViewModels;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Queries.GetUserPhoto;

public class GetUserPhotoQueryHandler(
    IHostEnvironment hostEnvironment,
    IEmployeeRepository employeeRepository) : IRequestHandler<GetUserPhotoQuery, DocumentVm>
{
    public async Task<DocumentVm> Handle(GetUserPhotoQuery request, CancellationToken cancellationToken = default)
    {
        var employee = await employeeRepository.GetById(request.EmployeeId, cancellationToken);

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

        return new DocumentVm
        {
            Name = employee.PhotoFullName,
            Path = photoPath,
            Extension = Path.GetExtension(employee.PhotoFullName)
        };
    }
}