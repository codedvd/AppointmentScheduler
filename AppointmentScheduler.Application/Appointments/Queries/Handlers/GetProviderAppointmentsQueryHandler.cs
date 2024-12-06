using MediatR;
using AppointmentScheduler.Infrastructure.Data;
using AppointmentScheduler.Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AppointmentScheduler.Application.Appointments.Queries.Handlers
{
    public class GetProviderAppointmentsQueryHandler : IRequestHandler<GetProviderAppointmentsQuery, ApiResponse<IEnumerable<AppointmentDto>>>
    {
        private readonly SchedulerDbContext _context;

        public GetProviderAppointmentsQueryHandler(SchedulerDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<IEnumerable<AppointmentDto>>> Handle(GetProviderAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<IEnumerable<AppointmentDto>>();
            var appointments = await _context.Appointments
                .Where(a => a.ProviderId == request.ProviderId)
                .Select(a => new FetchAppointmentDto
                {
                    PatientName = a.FullName,
                    PatientEmail = a.PatientEmail,
                    AppointmentDateTime = a.AppointmentDateTime,
                    ReasonForAppointment = a.ReasonForAppointment ?? "Unknown",
                    Duration = a.Duration
                })
                .ToListAsync(cancellationToken);
            if(appointments == null || appointments.Count < 1)
            {
                response.isSuccess = false;
                response.ResponseCode = "06";
                response.Message = $"Appointment not found for provider with id: {request.ProviderId}";
                return response;
            }

            response.isSuccess = true;
            response.ResponseCode = "00";
            response.Message = "Appointment retrieved successfully";
            response.Data = (IEnumerable<AppointmentDto>)appointments;
            return response;
        }
    }
}
