using ShiftTrack.Application.Modules.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.Queries.GetDepartmentById;

public record GetDepartmentByIdQuery(
    long Id) : IRequest<DepartmentVm>;