using MediatR;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommand : IRequest
    {
        public long Id { get; set; }
    }
}
