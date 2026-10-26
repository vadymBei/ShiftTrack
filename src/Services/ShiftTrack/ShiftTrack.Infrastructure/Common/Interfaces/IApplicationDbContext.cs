using Microsoft.EntityFrameworkCore;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ShiftTrack.Domain.Common.Outbox;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Domain.Modules.Communication.Notifications.Entities;
using ShiftTrack.Domain.Modules.Organization.Payrolls.Entities;
using ShiftTrack.Domain.Modules.Organization.Structure.Entities;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Entities;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;
using ShiftTrack.Domain.Modules.System.User.Roles.Entities;

namespace ShiftTrack.Infrastructure.Common.Interfaces;

public interface IApplicationDbContext
{
    //Booking
    //BusinessTrips
    DbSet<BusinessTrip> BusinessTrips { get; set; }
    DbSet<BusinessTripLocation> BusinessTripLocations { get; set; }
    
    //Vacations
    DbSet<Vacation> Vacations { get; set; }
    DbSet<VacationAccrual> VacationAccruals { get; set; }

    //Outbox
    DbSet<OutboxMessage> OutboxMessages { get; set; }

    DatabaseFacade Database { get; }

    //Communication
    //Notifications
    DbSet<Notification> Notifications { get; set; }
    
    //Organization
    //Structure
    DbSet<Unit> Units { get; set; }
    DbSet<Department> Departments { get; set; }
    DbSet<Position> Positions { get; set; }
    
    //Timesheet
    DbSet<Shift> Shifts { get; set; }
    DbSet<EmployeeShift> EmployeeShifts { get; set; }
    DbSet<EmployeeShiftHistory> EmployeeShiftHistories { get; set; }
    
    //Payrolls
    DbSet<Payroll> Payrolls { get; set; }
    
    //System
    //User
    DbSet<Employee> Employees { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<EmployeeRole> EmployeeRoles { get; set; }
    DbSet<EmployeeRoleUnit> EmployeeRoleUnits { get; set; }
    DbSet<EmployeeRoleUnitDepartment> EmployeeRoleUnitDepartments { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}