﻿using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Core.Domain.System.User.Employees.Entities;

namespace ShiftTrack.Core.Application.Data.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Unit> Units { get; set; }
    DbSet<Department> Departments { get; set; }
    DbSet<Position> Positions { get; set; }
    DbSet<Shift> Shifts { get; set; }
    DbSet<Employee> Employees { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}