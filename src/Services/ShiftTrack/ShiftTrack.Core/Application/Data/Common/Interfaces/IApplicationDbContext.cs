using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Domain.Booking.Vacations.Entities;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Core.Domain.System.User.EmployeeRoles.Entities;
using ShiftTrack.Core.Domain.System.User.Employees.Entities;
using ShiftTrack.Core.Domain.System.User.Roles.Entities;

namespace ShiftTrack.Core.Application.Data.Common.Interfaces;

public interface IApplicationDbContext
{
    //Booking
    //Vacations
    DbSet<Vacation> Vacations { get; set; }
    
    //Organization
    //Structure
    DbSet<Unit> Units { get; set; }
    DbSet<Department> Departments { get; set; }
    DbSet<Position> Positions { get; set; }
    
    //Timesheet
    DbSet<Shift> Shifts { get; set; }
    
    //System
    //User
    DbSet<Employee> Employees { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<EmployeeRole> EmployeeRoles { get; set; }
    DbSet<EmployeeRoleUnit> EmployeeRoleUnits { get; set; }
    DbSet<EmployeeRoleUnitDepartment> EmployeeRoleUnitDepartments { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}