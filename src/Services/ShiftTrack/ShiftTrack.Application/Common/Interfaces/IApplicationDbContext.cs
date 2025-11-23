using Microsoft.EntityFrameworkCore;
using ShiftTrack.Domain.Features.Booking.BusinessTrips.Entities;
using ShiftTrack.Domain.Features.Booking.Vacations.Entities;
using ShiftTrack.Domain.Features.Organization.Structure.Entities;
using ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Domain.Features.System.User.EmployeeRoles.Entities;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;
using ShiftTrack.Domain.Features.System.User.Roles.Entities;

namespace ShiftTrack.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    //Booking
    //BusinessTrips
    DbSet<BusinessTrip> BusinessTrips { get; set; }
    DbSet<BusinessTripParticipant> BusinessTripParticipants { get; set; }
    
    //Vacations
    DbSet<Vacation> Vacations { get; set; }
    
    //Organization
    //Structure
    DbSet<Unit> Units { get; set; }
    DbSet<Department> Departments { get; set; }
    DbSet<Position> Positions { get; set; }
    
    //Timesheet
    DbSet<Shift> Shifts { get; set; }
    DbSet<EmployeeShift> EmployeeShifts { get; set; }
    DbSet<EmployeeShiftHistory> EmployeeShiftHistories { get; set; }
    
    //System
    //User
    DbSet<Employee> Employees { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<EmployeeRole> EmployeeRoles { get; set; }
    DbSet<EmployeeRoleUnit> EmployeeRoleUnits { get; set; }
    DbSet<EmployeeRoleUnitDepartment> EmployeeRoleUnitDepartments { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}