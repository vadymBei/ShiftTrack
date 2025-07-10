using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Departments.Queries.GetDepartmentsByUnitId;

public record GetDepartmentsByUnitIdQuery(
    long UnitId) : IRequest<IEnumerable<DepartmentVm>>;