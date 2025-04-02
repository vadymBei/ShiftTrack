using ShiftTrack.Core.Application.System.User.Common.Dtos;

namespace ShiftTrack.Core.Application.System.User.Common.Interfaces;

public interface IEmployeeRoleService
{
    Task CreateEmployeeRole(EmployeeRoleToCreateDto dto, CancellationToken cancellationToken);
}