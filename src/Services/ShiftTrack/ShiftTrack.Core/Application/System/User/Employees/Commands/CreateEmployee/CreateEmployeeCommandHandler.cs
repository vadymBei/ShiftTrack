using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Core.Domain.System.User.Employees.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.System.User.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler(
    IMapper mapper,
    IEmployeeService userService,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<CreateEmployeeCommand, EmployeeVM>
{
    public async Task<EmployeeVM> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employeeAlreadyExist = await applicationDbContext.Employees
            .AnyAsync(x => x.PhoneNumber == request.PhoneNumber, cancellationToken);

        if (employeeAlreadyExist)
        {
            throw new UserAlreadyExistException(request.PhoneNumber);
        }
        
        var user = await userService.RegisterAuthUser(
            new UserToRegisterDto(
                request.PhoneNumber,
                request.Email,
                request.Password),
            cancellationToken);

        var employee = new Employee
        {
            Name = request.Name,
            Surname = request.Surname,
            Patronymic = request.Patronymic,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Gender = request.Gender,
            IntegrationId = user.Id
        };

        applicationDbContext.Employees.Add(employee);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<EmployeeVM>(employee);
    }
}