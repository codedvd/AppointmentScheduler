using Microsoft.EntityFrameworkCore;
using AppointmentScheduler.Infrastructure.Data;
using AppointmentScheduler.API.Extensions;
using AppointmentScheduler.API.Middleware;
using Serilog;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using FluentValidation;
using AppointmentScheduler.Application.Appointments.Commands.Handlers;
using AppointmentScheduler.Application.Appointments.Queries.Handlers;
using AppointmentScheduler.Application.Appointments.Commands;
using AppointmentScheduler.Application.Appointments.Queries;
using AppointmentScheduler.Application.Appointments.Commands.CommandValidators;

var builder = WebApplication.CreateBuilder(args);

// Configure Logging
ConfigureLogging(builder);
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();
ConfigurePipeline(app, app.Environment);

app.Run();

/// <summary>
/// Configure application logging
/// </summary>
static void ConfigureLogging(WebApplicationBuilder builder)
{
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .CreateLogger();

    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(dispose: true);
}

/// <summary>
/// Configure dependency injection and services
/// </summary>
static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAppointmentQueryHandler).Assembly));
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SetProviderAvailabilityCommandHandler).Assembly));
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateAppointmentCommandHandler).Assembly));

    services.AddValidatorsFromAssembly(typeof(BookAppointmentCommandValidator).Assembly);
    
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    services.AddDbContext<SchedulerDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

    services.AddInfrastructureServices(configuration);
    services.AddApplicationServices();
    services.AddApiServices();
} 

/// <summary>
/// Configure middleware and request pipeline
/// </summary>
static void ConfigurePipeline(WebApplication app, IWebHostEnvironment env)
{
    // Exception handling middleware
    app.UseMiddleware<GlobalExceptionMiddleware>();

    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Appointment Scheduler API v1"));
    }

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseCors("AllowAll");

    // Use top-level route registrations
    app.MapControllers();
    app.MapHealthChecks("/health");
}
