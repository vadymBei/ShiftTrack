using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.CreateDepartment
{
    public class CreateDepartmentCommand : IRequest<DepartmentVM>
    {
        public string Name { get; set; }

        public long UnitId { get; set; }
    }
}
