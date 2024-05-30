using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Queries.GetDepartmentsByUnitId
{
    public record GetDepartmentsByUnitIdQuery(
        long UnitId) : IRequest<IEnumerable<DepartmentVM>>;
    
}
