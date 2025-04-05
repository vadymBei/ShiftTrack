﻿using ShiftTrack.Data.Interfaces;

namespace ShiftTrack.Core.Domain.Organization.Structure.Entities;

public class Department : ISoftDeletable
{
    public long Id { get; set; }
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public long? UnitId { get; set; }
    public Unit Unit { get; set; }
}