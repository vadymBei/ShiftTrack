﻿using MediatR;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.ViewModels.Shifts;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Enums;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.UpdateShift;

public record UpdateShiftCommand(
    long Id,
    string Code,
    string Description,
    string Color,
    ShiftType Type,
    TimeSpan? StartTime,
    TimeSpan? EndTime) : IRequest<ShiftVM>;