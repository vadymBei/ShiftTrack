﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Authentication.Interfaces;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Core.Domain.System.User.Employees.Entities;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.System.User.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler(
    IMapper mapper,
    IPositionService positionService,
    IEmployeeService employeeService,
    IDepartmentService departmentService,
    ICurrentUserService currentUserService,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<UpdateEmployeeCommand, EmployeeVM>
{
    public async Task<EmployeeVM> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await applicationDbContext.Employees
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (employee == null)
        {
            throw new EntityNotFoundException(typeof(Employee), request.Id);
        }

        if (employee.PhoneNumber != request.PhoneNumber
            || employee.Email != request.Email)
        {
            var updatedUser = await employeeService.UpdateAuthUser(
                new UserToUpdateDto(
                    currentUserService.User.Id,
                    request.Email,
                    request.PhoneNumber)
                , cancellationToken);
        }

        employee.Name = request.Name;
        employee.Surname = request.Surname;
        employee.Patronymic = request.Patronymic;
        employee.Email = request.Email;
        employee.PhoneNumber = request.PhoneNumber;
        employee.DateOfBirth = request.DateOfBirth;
        employee.Gender = request.Gender;

        if (request.DepartmentId is not null)
        {
            await departmentService
                .GetById(request.DepartmentId, cancellationToken);

            employee.DepartmentId = request.DepartmentId;
        }
        else
        {
            employee.DepartmentId = null;
        }

        if (request.PositionId is not null)
        {
            await positionService
                .GetById(request.PositionId, cancellationToken);

            employee.PositionId = request.PositionId;
        }
        else
        {
            employee.PositionId = null;
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        employee = await employeeService
            .GetById(request.Id, cancellationToken);

        return mapper.Map<EmployeeVM>(employee);
    }
}