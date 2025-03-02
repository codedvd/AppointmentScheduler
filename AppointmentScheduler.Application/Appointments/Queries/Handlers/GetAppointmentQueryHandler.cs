using AppointmentScheduler.Domain.DTOs;
using AppointmentScheduler.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Application.Appointments.Queries.Handlers
{
    public class GetAppointmentQueryHandler : IRequestHandler<GetAppointmentQuery, ApiResponse<AppointmentDto>>
    {
        private readonly SchedulerDbContext _context;

        public GetAppointmentQueryHandler(SchedulerDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<AppointmentDto>> Handle(GetAppointmentQuery request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<AppointmentDto>();
            var appointment = await _context.Appointments
             .Include(a => a.Provider)
             .Where(a => a.Id == request.AppointmentId) 
             .Select(a => new AppointmentDto(
                 a.Id,
                 a.FullName,    
                 a.DateOfBirth, 
                 a.NationalId, 
                 a.PatientEmail,
                 a.ProviderId,
                 a.Provider.Name,       
                 a.AppointmentDateTime,
                 a.ReasonForAppointment,
                 a.Status
             ))
             .FirstOrDefaultAsync(cancellationToken);
            if(appointment == null)
            {
                response.isSuccess = false;
                response.Message = "Appointment does not exist";
                response.ResponseCode = "06";
                return response;
            }
            response.isSuccess = true;
            response.Message = $"Appointment with ID {appointment.Id} found";
            response.ResponseCode = "00";
            response.Data = appointment;
            return response;
        }
    }




    public class GetAppointmentQueryValidator : AbstractValidator<GetAppointmentQuery>
    {
        public GetAppointmentQueryValidator()
        {
            RuleFor(x => x.AppointmentId)
                .NotEmpty()
                .WithMessage("Appointment ID must be provided");
        }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}
