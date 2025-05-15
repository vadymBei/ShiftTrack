using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Queries.GetDepartmentsByUnitId;

public record GetDepartmentsByUnitIdQuery(
    long UnitId) : IRequest<IEnumerable<DepartmentVM>>;