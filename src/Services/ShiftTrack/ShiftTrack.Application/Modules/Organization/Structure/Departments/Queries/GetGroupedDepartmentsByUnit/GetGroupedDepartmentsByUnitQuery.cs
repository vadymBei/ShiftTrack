using ShiftTrack.Application.Modules.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.Queries.GetGroupedDepartmentsByUnit;

public record GetGroupedDepartmentsByUnitQuery() : IRequest<IEnumerable<GroupedDepartmentsByUnitVm>>;