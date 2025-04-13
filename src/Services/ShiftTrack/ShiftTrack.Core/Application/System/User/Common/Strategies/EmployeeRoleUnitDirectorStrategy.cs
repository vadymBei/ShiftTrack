using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Domain.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Core.Application.System.User.Common.Strategies;

public sealed class EmployeeRoleUnitDirectorStrategy(
    IApplicationDbContext applicationDbContext) : IEmployeeRoleStrategy
{
    #region EmployeeRole

    public Task<EmployeeRole> GetEmployeeRoleById(long employeeRoleId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<EmployeeRole> CreateEmployeeRole(
        EmployeeRoleToCreateDto dto,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteEmployeeRole(long employeeRoleId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<EmployeeRole>> GetEmployeeRolesByEmployeeId(long employeeId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    #endregion

    #region EmployeeRoleUnit

    public Task<EmployeeRoleUnit> CreateEmployeeRoleUnit(EmployeeRoleUnitToCreateDto dto,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteEmployeeRoleUnit(long employeeUnitId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region EmployeeRoleUnitDepartment

    public Task<IEnumerable<EmployeeRoleUnitDepartment>> CreateEmployeeRoleUnitDepartments(
        EmployeeRoleUnitDepartmentsToCreateDto dto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteEmployeeRoleUnitDepartment(long employeeRoleUnitDepartmentId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    #endregion
}