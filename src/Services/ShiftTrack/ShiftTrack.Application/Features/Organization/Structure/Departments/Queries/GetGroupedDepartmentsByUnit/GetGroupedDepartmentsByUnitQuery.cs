using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Departments.Queries.GetGroupedDepartmentsByUnit;

public record GetGroupedDepartmentsByUnitQuery() : IRequest<IEnumerable<GroupedDepartmentsByUnitVm>>;