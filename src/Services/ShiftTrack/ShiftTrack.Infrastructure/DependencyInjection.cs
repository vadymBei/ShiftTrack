using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.Queries.DownloadVacationRequestPdf;
using ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.Queries.ExportTimesheet;
using ShiftTrack.Application.Modules.System.Auth.Common.Interfaces;
using ShiftTrack.Application.Modules.System.User.Common.Interfaces;
using ShiftTrack.Client.Http;
using ShiftTrack.Infrastructure.Common.Repositories;
using ShiftTrack.Infrastructure.Common.Services;
using ShiftTrack.Infrastructure.Common.Services.Excel;
using ShiftTrack.Infrastructure.Common.Services.Pdf;
using ShiftTrack.Infrastructure.Modules.System.Auth.Repositories;
using ShiftTrack.Infrastructure.Modules.System.User.Repositories;
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
        services.AddTransient<IExcelExporter<TimesheetExportData>, TimesheetPlanExporter>();
        services.AddTransient<IPdfExporter<VacationRequestData>, VacationRequestExporter>();

        //Repositories
        services.AddTransient<IPdfRepository, PdfRepository>();

        //System
        //Auth
        services.AddTransient<ITokenRepository, TokenRepository>();
        
        //User
        services.AddTransient<IUserRepository, UserRepository>();
        
        return services;
    }
}