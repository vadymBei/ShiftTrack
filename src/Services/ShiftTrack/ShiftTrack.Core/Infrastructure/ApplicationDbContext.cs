using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Core.Domain.System.User.Employees.Entities;
using ShiftTrack.Core.Infrastructure.Interceptors;
using System.Reflection;
using ShiftTrack.Core.Domain.System.User.EmployeeRoles.Entities;
using ShiftTrack.Core.Domain.System.User.Roles.Entities;

namespace ShiftTrack.Core.Infrastructure;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            
        ChangeTracker.AutoDetectChangesEnabled = true;
    }

    //Structure
    public DbSet<Unit> Units { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Position> Positions { get; set; }
    
    //Timesheet
    public DbSet<Shift> Shifts { get; set; }
    
    //User
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<EmployeeRole> EmployeeRoles { get; set; }
    public DbSet<EmployeeRoleUnit> EmployeeRoleUnits { get; set; }
    public DbSet<EmployeeRoleUnitDepartment> EmployeeRoleUnitDepartments { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new SoftDeleteInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}