using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Domain.Booking.Vacations.Entities;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Core.Domain.System.User.EmployeeRoles.Entities;
using ShiftTrack.Core.Domain.System.User.Employees.Entities;
using ShiftTrack.Core.Domain.System.User.Roles.Entities;
using ShiftTrack.Core.Infrastructure.Interceptors;
using ShiftTrack.Data.Interfaces;

namespace ShiftTrack.Core.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly ICurrentUserService _currentUserService;
    
    public ApplicationDbContext(
        ICurrentUserService currentUserService,
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        _currentUserService = currentUserService;
        
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            
        ChangeTracker.AutoDetectChangesEnabled = true;
    }

    //Booking
    //Vacations
    public DbSet<Vacation> Vacations { get; set; }
    
    //Organization
    //Structure
    public DbSet<Unit> Units { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Position> Positions { get; set; }
    
    //Timesheet
    public DbSet<Shift> Shifts { get; set; }
    
    //System
    //User
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<EmployeeRole> EmployeeRoles { get; set; }
    public DbSet<EmployeeRoleUnit> EmployeeRoleUnits { get; set; }
    public DbSet<EmployeeRoleUnitDepartment> EmployeeRoleUnitDepartments { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var entries = ChangeTracker.Entries<IAuditable>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);
        
        var currentTime = DateTime.UtcNow;
        
        var currentUserId = _currentUserService.Employee?.Id;
        
        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                {
                    entry.Entity.CreatedById = currentUserId;
                    entry.Entity.CreatedAt = currentTime;
                    break;
                }
                
                case EntityState.Modified:
                {
                    entry.Entity.ModifiedById = currentUserId;
                    entry.Entity.ModifiedAt = currentTime;
                    break;
                }
            }
        }

        
        return await base.SaveChangesAsync(cancellationToken);
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