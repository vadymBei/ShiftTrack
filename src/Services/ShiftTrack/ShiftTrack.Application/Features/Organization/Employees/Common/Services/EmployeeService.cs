using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Employees.Common.Interfaces;
using ShiftTrack.Application.Features.System.Auth.Common.Dtos;
using ShiftTrack.Application.Features.System.User.Common.Dtos;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Domain.Features.System.Auth.Models;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.Organization.Employees.Common.Services;

public class EmployeeService(
    IUserRepository userRepository,
    IApplicationDbContext applicationDbContext)
    : IEmployeeService
{
    public async Task<Token> ChangePassword(ChangePasswordDto dto, CancellationToken cancellationToken)
    {
        var employee = await GetById(dto.EmployeeId, cancellationToken);

        var token = await userRepository.ChangePassword(
            new ChangeUserPasswordDto(
                employee.IntegrationId,
                dto.OldPassword,
                dto.NewPassword,
                dto.ConfirmPassword),
            cancellationToken);

        return token;
    }

    public async Task<Employee> GetById(object id, CancellationToken cancellationToken)
    {
        long employeeId = (long)id;

        var employee = await applicationDbContext.Employees
            .AsNoTracking()
            .Include(x => x.Department)
            .ThenInclude(x => x.Unit)
            .Include(x => x.Position)
            .FirstOrDefaultAsync(x => x.Id == employeeId, cancellationToken);

        if (employee == null)
        {
            throw new EntityNotFoundException(typeof(Employee), employeeId);
        }

        return employee;
    }

    public async Task<Employee> GetByIntegrationId(string integrationId, CancellationToken cancellationToken)
    {
        var employee = await applicationDbContext.Employees
            .AsNoTracking()
            .Include(x => x.Position)
            .Include(x => x.Department)
            .ThenInclude(x => x.Unit)
            .Include(x => x.EmployeeRoles)
            .ThenInclude(x => x.Role)
            .Include(x => x.EmployeeRoles)
            .ThenInclude(x => x.Units)
            .ThenInclude(x => x.Departments)
            .ThenInclude(x => x.Department)
            .FirstOrDefaultAsync(x => x.IntegrationId == integrationId, cancellationToken);
        
        return employee;
    }

    public async Task<Employee> UpdateVacationDaysBalance(long employeeId, int vacationDaysBalance, CancellationToken cancellationToken)
    {
        var employee = await applicationDbContext.Employees
            .FirstOrDefaultAsync(x => x.Id == employeeId, cancellationToken)
            ?? throw new EntityNotFoundException(typeof(Employee), employeeId);
        
        employee.VacationDaysBalance = vacationDaysBalance;
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        
        return employee;
    }

    public Task<Authentication.Models.User> RegisterAuthUser(UserToRegisterDto dto, CancellationToken cancellationToken)
    {
        return userRepository.RegisterUser(dto, cancellationToken);
    }

    public Task<Authentication.Models.User> UpdateAuthUser(UserToUpdateDto dto, CancellationToken cancellationToken)
    {
        return userRepository.UpdateUser(dto, cancellationToken);
    }
}