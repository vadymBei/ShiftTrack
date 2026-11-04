using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Employees.UseCases.Queries.GetEmployeePhoto;

public record GetEmployeePhotoQuery(
    long EmployeeId) : IRequest<EmployeePhotoFileVm>;
