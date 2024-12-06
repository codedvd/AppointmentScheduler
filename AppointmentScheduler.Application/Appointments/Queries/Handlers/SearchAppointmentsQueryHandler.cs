using MediatR;
using AppointmentScheduler.Infrastructure.Data;
using AppointmentScheduler.Domain.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Application.Appointments.Queries.Handlers
{
    public class SearchAppointmentsQueryHandler : IRequestHandler<SearchAppointmentsQuery, ApiResponse<IEnumerable<FetchAppointmentDto>>>
    {
        private readonly SchedulerDbContext _context;

        public SearchAppointmentsQueryHandler(SchedulerDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<IEnumerable<FetchAppointmentDto>>> Handle(SearchAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<IEnumerable<FetchAppointmentDto>>();
            var query = _context.Appointments.AsQueryable();

            if (!string.IsNullOrEmpty(request.PatientName))
            {
                query = query.Where(a => a.FullName.ToLower().Contains(request.PatientName.ToLower()));
            }
           
            if (request.StartDate.HasValue && request.EndDate.HasValue)
            {
                query = query.Where(a => a.AppointmentDateTime >= request.StartDate.Value.ToUniversalTime() &&
                    a.AppointmentDateTime <= request.EndDate.Value.ToUniversalTime() && a.ProviderId == request.ProviderId);
            } 

            if (query.Count() < 1)
            {
                response.isSuccess = false;
                response.ResponseCode = "06";
                response.Message = "No appointment exist with the search parameters provided.";
                return response;
            } 
            var appointments = await query
                .Select(a => new FetchAppointmentDto
                {
                    PatientName = a.FullName,
                    PatientEmail = a.PatientEmail,
                    Status = a.Status,
                    Duration = a.Duration,
                    AppointmentDateTime = a.AppointmentDateTime.ToUniversalTime(),
                    ReasonForAppointment = a.ReasonForAppointment ?? "UnKnown"
                }).ToListAsync(cancellationToken);

            return new ApiResponse<IEnumerable<FetchAppointmentDto>>
            {
                isSuccess = true,
                ResponseCode = "00",
                Message = "Search Retrieved Successfully",
                Data = [.. appointments]
            };
        }
    }
}
