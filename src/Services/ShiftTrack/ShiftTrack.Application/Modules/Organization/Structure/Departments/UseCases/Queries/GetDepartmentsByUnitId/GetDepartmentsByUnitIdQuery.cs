using ShiftTrack.Application.Modules.Organization.Structure.Departments.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Queries.GetDepartmentsByUnitId;

public record GetDepartmentsByUnitIdQuery(
    long UnitId) : IRequest<IEnumerable<DepartmentVm>>;