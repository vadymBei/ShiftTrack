﻿
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.DeleteDepartment;

public record DeleteDepartmentCommand(
    long Id) : IRequest;