using ShiftTrack.Application.Modules.Organization.Structure.Departments.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Queries.GetDepartmentById;

public record GetDepartmentByIdQuery(
    long Id) : IRequest<DepartmentVm>;