using AppointmentScheduler.Domain.Models;
using AppointmentScheduler.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using FluentValidation;
using AppointmentScheduler.Application.Appointments.Commands.Handlers;
using AppointmentScheduler.Application.Appointments.Commands;
using System.Text.Json.Serialization;

namespace AppointmentScheduler.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Configure Email Settings
            services.Configure<EmailSettings>(
                configuration.GetSection("EmailSettings"));

            // Add Notification Service
            services.AddScoped<INotificationService, EmailNotificationService>();
            services.AddSingleton<SmtpClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<EmailSettings>>().Value;
                return new SmtpClient(settings.SmtpServer, settings.SmtpPort)
                {
                    Credentials = new NetworkCredential(settings.Username, settings.Password),
                    EnableSsl = true
                };
            });
            return services;
        }

        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            // MediatR
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            // Validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }

        public static IServiceCollection AddApiServices(
            this IServiceCollection services)
        {
            services.AddControllers();

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Appointment Scheduler API",
                    Version = "v1"
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            services.AddHealthChecks();
            return services;
        }
    }
}
