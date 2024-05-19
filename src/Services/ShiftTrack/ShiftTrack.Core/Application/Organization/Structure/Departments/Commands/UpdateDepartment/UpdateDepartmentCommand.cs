using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommand : IRequest<DepartmentVM>
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
