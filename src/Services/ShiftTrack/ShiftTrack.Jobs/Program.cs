using ShiftTrack.Application;
using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Infrastructure;
using ShiftTrack.Jobs.Jobs.Booking;
using ShiftTrack.Jobs.Services;
using ShiftTrack.Scheduling;
using ShiftTrack.Scheduling.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();

// Jobs don't have SignalR clients, notifications are only stored.
builder.Services.AddScoped<INotificationPublisher, NullNotificationPublisher>();

builder.Services.AddScheduling(builder.Configuration, jobs =>
{
    jobs.AddJob<VacationAccrualJob>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Not protected: the jobs host isn't routed through the gateway, don't publish its port in production.
app.MapSchedulingEndpoints();

app.Run();
