using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Queries.DownloadVacationRequestPdf;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.Organization.Payrolls.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShiftHistories.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.UseCases.Queries.ExportUnitTimesheet;
using ShiftTrack.Application.Modules.System.Auth.Account.Interfaces;
using ShiftTrack.Application.Modules.System.Auth.Tokens.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Interfaces;
using ShiftTrack.Application.Modules.System.User.Roles.Interfaces;
using ShiftTrack.Client.Http;
using ShiftTrack.Infrastructure.Common.Interfaces;
using ShiftTrack.Infrastructure.Common.Repositories;
using ShiftTrack.Infrastructure.Common.Services;
using ShiftTrack.Infrastructure.Common.Services.Excel;
using ShiftTrack.Infrastructure.Common.Services.Pdf;
using ShiftTrack.Infrastructure.Modules.Booking.BusinessTrips.Repositories;
using ShiftTrack.Infrastructure.Modules.Booking.Vacations.Repositories;
using ShiftTrack.Infrastructure.Modules.Organization.Employees.Repositories;
using ShiftTrack.Infrastructure.Modules.Organization.Payrolls.Repositories;
using ShiftTrack.Infrastructure.Modules.Organization.Structure.Departments.Repositories;
using ShiftTrack.Infrastructure.Modules.Organization.Structure.Positions.Repositories;
using ShiftTrack.Infrastructure.Modules.Organization.Structure.Units.Repositories;
using ShiftTrack.Infrastructure.Modules.Organization.Timesheet.EmployeeShiftHistories.Repositories;
using ShiftTrack.Infrastructure.Modules.Organization.Timesheet.EmployeeShifts.Repositories;
using ShiftTrack.Infrastructure.Modules.Organization.Timesheet.Shifts.Repositories;
using ShiftTrack.Infrastructure.Modules.System.Auth.Account.Repositories;
using ShiftTrack.Infrastructure.Modules.System.Auth.Tokens.Repositories;
using ShiftTrack.Infrastructure.Modules.System.User.EmployeeRoles.Repositories;
using ShiftTrack.Infrastructure.Modules.System.User.EmployeeRoleUnitDepartments.Repositories;
using ShiftTrack.Infrastructure.Modules.System.User.EmployeeRoleUnits.Repositories;
using ShiftTrack.Infrastructure.Modules.System.User.Roles.Repositories;
using ShiftTrack.Infrastructure.Persistence;
using ShiftTrack.Kernel.Attributes;

namespace ShiftTrack.Infrastructure;

[ShiftTrackMember]
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddClientHttp(configuration);
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        //Services
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddTransient<IExcelExporter<UnitTimesheetExportData>, TimesheetPlanExporter>();
        services.AddTransient<IPdfExporter<VacationRequestData>, VacationRequestExporter>();

        //Repositories
        services.AddTransient<IPdfRepository, PdfRepository>();

        //Booking
        //BusinessTrips
        services.AddTransient<ILocationRepository, LocationRepository>();
        services.AddTransient<IBusinessTripRepository, BusinessTripRepository>();
        
        //Vacations
        services.AddTransient<IVacationRepository, VacationRepository>();
        
        //User
        services.AddTransient<IRoleRepository, RoleRepository>();
        services.AddTransient<IEmployeeRoleRepository, EmployeeRoleRepository>();
        services.AddTransient<IEmployeeRoleUnitRepository, EmployeeRoleUnitRepository>();
        services.AddTransient<IEmployeeRoleUnitDepartmentRepository, EmployeeRoleUnitDepartmentRepository>();
        
        //Organization
        //Structure
        services.AddTransient<IDepartmentRepository, DepartmentRepository>();
        services.AddTransient<IUnitRepository, UnitRepository>();
        services.AddTransient<IPositionRepository, PositionRepository>();
        services.AddTransient<IEmployeeRepository, EmployeeRepository>();
        
        //Timesheet
        services.AddTransient<IShiftRepository, ShiftRepository>();
        services.AddTransient<IEmployeeShiftHistoryRepository, EmployeeShiftHistoryRepository>();
        services.AddTransient<IEmployeeShiftRepository, EmployeeShiftRepository>();
        
        //Payrolls
        services.AddTransient<IPayrollRepository, PayrollRepository>();
        
        //System
        //Auth
        services.AddTransient<ITokenRepository, TokenRepository>();
        
        //User
        services.AddTransient<IAccountRepository, AccountRepository>();
        services.AddTransient<IRoleRepository, RoleRepository>();
        
        return services;
    }
}