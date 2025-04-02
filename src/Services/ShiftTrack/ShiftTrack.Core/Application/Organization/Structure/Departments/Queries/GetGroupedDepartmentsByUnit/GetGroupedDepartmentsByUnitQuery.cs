using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Queries.GetGroupedDepartmentsByUnit;

public record GetGroupedDepartmentsByUnitQuery() : IRequest<IEnumerable<GroupedDepartmentsByUnitVM>>;