using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Employees.UseCases.Queries.GetEmployeePhotos;

/// <summary>Which of the employees have a photo, with its version.</summary>
public record GetEmployeePhotosQuery(
    IReadOnlyCollection<long> EmployeeIds) : IRequest<IReadOnlyList<EmployeePhotoVm>>;
