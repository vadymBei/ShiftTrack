﻿using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Enums;
using ShiftTrack.Data.Interfaces;

namespace ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Entities;

public class Shift : ISoftDeletable
{
    public long Id { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public TimeSpan? WorkHours { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public ShiftType Type { get; set; }
}