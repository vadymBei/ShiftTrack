using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.UseCases.Commands.DeleteEmployeeRoleUnit;

public class DeleteEmployeeRoleUnitCommandHandler(
    IEmployeeRoleUnitService employeeRoleUnitService) : IRequestHandler<DeleteEmployeeRoleUnitCommand>
{
    public async Task Handle(DeleteEmployeeRoleUnitCommand request, CancellationToken cancellationToken)
    {
        await employeeRoleUnitService.Delete(
            request.Id,
            cancellationToken);
    }
}