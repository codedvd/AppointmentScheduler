using AppointmentScheduler.Application.Appointments.Queries;
using AppointmentScheduler.Domain.DTOs;
using AppointmentScheduler.Domain.Models;
using AppointmentScheduler.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetProviderQueryHandler : IRequestHandler<GetProviderQuery, ApiResponse<ProviderDto>>
{
    private readonly SchedulerDbContext _context;

    public GetProviderQueryHandler(SchedulerDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<ProviderDto>> Handle(GetProviderQuery request, CancellationToken cancellationToken)
    {
        var response = new ApiResponse<ProviderDto>();
        var provider = await _context.Providers
            .Include(p => p.Appointments)
            .Include(p => p.Availabilities)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.ProviderId, cancellationToken);

        if (provider == null)
        {
            response.isSuccess = false;
            response.ResponseCode = "06";
            response.Message = "Provider not found.";
            return response;
        }

        var appointments = provider.Appointments
            .Where(a => a.Status != AppointmentStatus.Cancelled && a.Status != AppointmentStatus.Completed)
            .Select(a => new FetchAppointmentDto
            {
                AppointmentDateTime = a.AppointmentDateTime,
                PatientEmail = a.PatientEmail,
                Duration = a.Duration,
                PatientName = a.FullName,
                ReasonForAppointment = a.ReasonForAppointment ?? "Unknown",
                Status = a.Status
            })
            .ToList();

        var availabilities = provider.Availabilities
            .Where(a => a.StartTime > DateTime.Now.TimeOfDay) 
            .Select(a => new AvailabilityDto
            {
                DayOfWeek = a.DayOfWeek,
                StartTime = a.StartTime,
                EndTime = a.EndTime
            })
            .ToList();

        var final =  new ProviderDto
        {
            Name = provider.Name,
            Specialty = provider.Specialty,
            Email = provider.Email,
            Phone = provider.Phone,
            Appointments = appointments, 
            Availabilities = availabilities
        };

        response.isSuccess = true;
        response.ResponseCode = "00";
        response.Message = "Providers returned successfully";
        response.Data = final;
        return response;
    }
}
