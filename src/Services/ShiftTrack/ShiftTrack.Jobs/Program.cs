using ShiftTrack.Application;
using ShiftTrack.Infrastructure;
using ShiftTrack.Infrastructure.Realtime;
using ShiftTrack.Jobs.Jobs.Booking;
using ShiftTrack.Jobs.Jobs.Outbox;
using ShiftTrack.Scheduling;
using ShiftTrack.Scheduling.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();

// No hub here: notifications are sent to the clients of the API through the Redis backplane.
builder.Services.AddRealtime(builder.Configuration);

builder.Services.AddScheduling(builder.Configuration, jobs =>
{
    jobs.AddJob<VacationAccrualJob>();
    jobs.AddJob<OutboxProcessorJob>();
    jobs.AddJob<OutboxCleanupJob>();
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
