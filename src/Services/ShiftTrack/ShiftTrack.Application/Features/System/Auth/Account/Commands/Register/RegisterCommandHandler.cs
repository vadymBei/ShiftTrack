using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Employees.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Employees.Common.ViewModels;
using ShiftTrack.Application.Features.System.User.Common.Dtos;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.System.Auth.Account.Commands.Register;

public class RegisterCommandHandler(
    IMapper mapper,
    IEmployeeService userService,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<RegisterCommand, EmployeeVm>
{
    public async Task<EmployeeVm> Handle(RegisterCommand request, CancellationToken cancellationToken = default)
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

        return mapper.Map<EmployeeVm>(employee);
    }
}