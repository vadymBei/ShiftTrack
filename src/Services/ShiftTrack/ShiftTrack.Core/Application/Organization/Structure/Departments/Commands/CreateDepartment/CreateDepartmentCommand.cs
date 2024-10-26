using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.CreateDepartment
{
    public record CreateDepartmentCommand(
        string Name,
        long UnitId) : IRequest<DepartmentVM>;
}
