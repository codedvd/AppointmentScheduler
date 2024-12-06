using MediatR;
using AppointmentScheduler.Infrastructure.Data;
using AppointmentScheduler.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using AppointmentScheduler.Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using AppointmentScheduler.Infrastructure.Services;

namespace AppointmentScheduler.Application.Appointments.Commands.Handlers
{

    public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand, ApiResponse<bool>>
    {
        private readonly SchedulerDbContext _context;
        private readonly INotificationService _notificationService;
        public UpdateAppointmentCommandHandler(SchedulerDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<ApiResponse<bool>> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<bool>();
            var appointment = await _context.Appointments.FirstOrDefaultAsync(p => p.ProviderId == request.ProviderId && p.Id == request.AppointmentId);
            if (appointment == null)
            {
                response.isSuccess = false;
                response.ResponseCode = "06";
                response.Message = "Appointment not found";
                return response;
            }

            appointment.Status = request.AppointmentStatus;
            await _context.SaveChangesAsync(cancellationToken);

            //send email
            switch (request.AppointmentStatus)
            {
                case AppointmentStatus.Cancelled:
                    await _notificationService.SendAppointmentCancellationAsync(appointment);
                    break;
                case AppointmentStatus.Confirmed:
                    await _notificationService.SendAppointmentReminderAsync(appointment);
                    break;
                case AppointmentStatus.Completed:
                    await _notificationService.SendAppointmentCompletionAsync(appointment);
                    break;
            }
            response.isSuccess = true;
            response.ResponseCode = "200";
            response.Message = "Appointment successfully updated";
            return response;
        }
    }
}