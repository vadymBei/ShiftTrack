using ShiftTrack.Application.Modules.Organization.Structure.Departments.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Queries.GetGroupedDepartmentsByUnit;

public record GetGroupedDepartmentsByUnitQuery() : IRequest<IEnumerable<GroupedDepartmentsByUnitVm>>;