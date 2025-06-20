﻿using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoleUnitDepartments.Commands.CreateEmployeeRoleUnitDepartment;

public record CreateEmployeeRoleUnitDepartmentCommand(
    long DepartmentId,
    long EmployeeRoleUnitId) : IRequest<EmployeeRoleUnitDepartmentVm>;