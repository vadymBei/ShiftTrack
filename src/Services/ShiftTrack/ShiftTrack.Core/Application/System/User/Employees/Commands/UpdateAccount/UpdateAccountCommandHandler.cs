using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Core.Domain.System.User.Employees.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.System.User.Employees.Commands.UpdateAccount;

public class UpdateAccountCommandHandler(
    IMapper mapper,
    IEmployeeService employeeService,
    ICurrentUserService currentUserService,
    IApplicationDbContext applicationDbContext) : IRequestHandler<UpdateAccountCommand, EmployeeVM>
{
    public async Task<EmployeeVM> Handle(UpdateAccountCommand request, CancellationToken cancellationToken = default)
    {
        var employee = await applicationDbContext.Employees
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (employee == null
            || employee.IntegrationId != currentUserService.Employee.IntegrationId)
        {
            throw new EntityNotFoundException(typeof(Employee), request.Id);
        }
        
        if (employee.PhoneNumber != request.PhoneNumber
            || employee.Email != request.Email)
        {
            var user = await employeeService.UpdateAuthUser(
                new UserToUpdateDto(
                    currentUserService.Employee.IntegrationId,
                    request.Email,
                    request.PhoneNumber)
                , cancellationToken);
            
            employee.Email = user.Email;
            employee.PhoneNumber = user.PhoneNumber;
        }
        
        employee.Name = request.Name;
        employee.Surname = request.Surname;
        employee.Patronymic = request.Patronymic;
        employee.DateOfBirth = request.DateOfBirth;
        employee.Gender = request.Gender;
        
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        employee = await employeeService
            .GetById(request.Id, cancellationToken);

        return mapper.Map<EmployeeVM>(employee);
    }
}