using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Common.ViewModels;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.System.Auth.Account.Commands.UploadPhoto;

public class UploadPhotoCommandHandler(
    IHostEnvironment hostEnvironment, 
    IApplicationDbContext applicationDbContext) : IRequestHandler<UploadPhotoCommand, DocumentVm>
{
    public async Task<DocumentVm> Handle(UploadPhotoCommand request, CancellationToken cancellationToken = default)
    {
        var employee = await applicationDbContext.Employees
                           .FirstOrDefaultAsync(x => x.Id == request.EmployeeId, cancellationToken)
                       ?? throw new EntityNotFoundException(typeof(Employee), request.EmployeeId);

        var document = await UploadPhotoToFolder(request.File, cancellationToken);

        if (!string.IsNullOrEmpty(employee.PhotoFullName))
        {
            DeletePreviousPhoto(employee.PhotoFullName);
        }

        employee.PhotoFullName = document.Name;

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return document;
    }

    private async Task<DocumentVm> UploadPhotoToFolder(IFormFile file, CancellationToken cancellationToken)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
            throw new Exception("Only .jpg, .jpeg and .png files are allowed");

        if (file.Length > 5 * 1024 * 1024)
            throw new Exception("File size must be less than 5MB");
        
        var documentVm = new DocumentVm
        {
            Extension = extension
        };
        
        try
        {
            var uploadsFolder = GetOrCreateUploadsEmployeesFolder();

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(stream, cancellationToken);

            documentVm.Path = filePath;
            documentVm.Name = fileName;
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to upload photo. {e.InnerException?.Message}");
        }

        return documentVm;
    }

    private string GetOrCreateUploadsEmployeesFolder()
    {
        var uploadsFolder = Path.Combine(
            hostEnvironment.ContentRootPath, 
            "wwwroot",
            "uploads");

        if (!Directory.Exists(uploadsFolder))
        {
            try
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to create uploads folder. {e.InnerException?.Message}");
            }
        }

        uploadsFolder = Path.Combine(uploadsFolder, "employees");

        if (!Directory.Exists(uploadsFolder))
        {
            try
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to create uploads employees folder. {e.InnerException?.Message}");
            }
        }

        return uploadsFolder;
    }

    private void DeletePreviousPhoto(string photoFullName)
    {
        try
        {
            var photoPath = Path.Combine(
                hostEnvironment.ContentRootPath, 
                "wwwroot", 
                "uploads",
                "employees",
                photoFullName);

            if (File.Exists(photoPath))
                File.Delete(photoPath);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to delete photo {photoFullName}. {ex.InnerException?.Message}");
        }
    }
}