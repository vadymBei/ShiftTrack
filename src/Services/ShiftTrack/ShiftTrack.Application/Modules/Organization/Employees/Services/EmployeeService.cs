using ShiftTrack.Application.Modules.Organization.Employees.Dtos;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Interfaces;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Application.Modules.Organization.Employees.Services;

public class EmployeeService(
    IUnitRepository unitRepository,
    IPositionRepository positionRepository,
    IEmployeeRepository employeesRepository,
    IDepartmentRepository departmentRepository) : IEmployeeService
{
    public async Task<IEnumerable<Employee>> GetEmployees(
        EmployeesFilterDto filter,
        CancellationToken cancellationToken)
    {
        if (filter.UnitId is not null)
        {
            await unitRepository.GetById((long)filter.UnitId, cancellationToken);
        }

        if (filter.DepartmentId is not null)
        {
            await departmentRepository.GetById((long)filter.DepartmentId, cancellationToken);
        }

        var employees = await employeesRepository.GetFiltered(filter, cancellationToken);

        return employees;
    }

    public async Task<Employee> Update(EmployeeToUpdateDto employeeToUpdateDto, CancellationToken cancellationToken)
    {
        if (employeeToUpdateDto.DepartmentId is not null)
        {
            await departmentRepository
                .GetById((long)employeeToUpdateDto.DepartmentId, cancellationToken);
        }

        if (employeeToUpdateDto.PositionId is not null)
        {
            await positionRepository
                .GetById((long)employeeToUpdateDto.PositionId, cancellationToken);
        }

        await employeesRepository.Update(employeeToUpdateDto, cancellationToken);

        return await employeesRepository.GetById(employeeToUpdateDto.Id, cancellationToken);
    }
}