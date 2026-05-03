using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Commands.DeleteDepartment;

public class DeleteDepartmentCommandHandler(
    IDepartmentRepository departmentRepository) 
    : IRequestHandler<DeleteDepartmentCommand>
{
    public async Task Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
       await departmentRepository.Delete(request.Id, cancellationToken);
    }
}