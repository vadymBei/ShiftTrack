using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Models;
using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Employees.UseCases.Queries.GetEmployeePhotos;

public class GetEmployeePhotosQueryHandler(
    IEmployeeRepository employeeRepository) : IRequestHandler<GetEmployeePhotosQuery, IReadOnlyList<EmployeePhotoVm>>
{
    public async Task<IReadOnlyList<EmployeePhotoVm>> Handle(GetEmployeePhotosQuery request,
        CancellationToken cancellationToken = default)
    {
        var ids = request.EmployeeIds.Where(x => x > 0).Distinct().ToList();

        if (ids.Count == 0)
            return [];

        var photos = await employeeRepository.GetPhotoNames(ids, cancellationToken);

        return photos
            .Select(x => new EmployeePhotoVm { EmployeeId = x.Key, Version = PhotoFormat.VersionOf(x.Value) })
            .OrderBy(x => x.EmployeeId)
            .ToList();
    }
}
