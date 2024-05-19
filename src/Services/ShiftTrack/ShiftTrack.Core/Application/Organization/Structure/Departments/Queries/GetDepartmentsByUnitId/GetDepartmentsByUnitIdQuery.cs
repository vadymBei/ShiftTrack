using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Queries.GetDepartmentsByUnitId
{
    public class GetDepartmentsByUnitIdQuery : IRequest<IEnumerable<DepartmentVM>>
    {
        public long UnitId { get; set; }
    }
}
