using AppointmentScheduler.Domain.DTOs;
using AppointmentScheduler.Domain.Models;
using AppointmentScheduler.Infrastructure.Data;
using AppointmentScheduler.Infrastructure.Services;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Application.Appointments.Commands.Handlers
{
    public class BookAppointmentCommandHandler : IRequestHandler<BookAppointmentCommand, ApiResponse<Guid>>
    {
        private readonly SchedulerDbContext _context;
        private readonly IValidator<BookAppointmentCommand> _validator;
        private readonly INotificationService _notificationService;

        public BookAppointmentCommandHandler(
            SchedulerDbContext context,
            IValidator<BookAppointmentCommand> validator,
            INotificationService notificationService)
        {
            _context = context;
            _validator = validator;
            _notificationService = notificationService;
        }

        public async Task<ApiResponse<Guid>> Handle(BookAppointmentCommand request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<Guid>();
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Check provider availability
            var provider = await _context.Providers
                .Include(p => p.Availabilities)
                .FirstOrDefaultAsync(p => p.Id == request.ProviderId, cancellationToken);

            if (provider == null)
            {
                response.isSuccess = false;
                response.Message = "Provider not found";
                response.ResponseCode = "06";
                return response;
            }

            // Check for overlapping appointments
            var existingAppointment = await _context.Appointments
                .AnyAsync(a =>
                    a.ProviderId == request.ProviderId &&
                    a.AppointmentDateTime == request.AppointmentDateTime,
                    cancellationToken);

            if (existingAppointment)
            {
                response.isSuccess = false;
                response.Message = "Appointment slot already booked";
                response.ResponseCode = "06";
                return response;
            }

            // Create appointment
            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                FullName = request.PatientName,
                DateOfBirth = request.DateOfBirth,
                NationalId = request.NationalId,
                PatientEmail = request.PatientEmail,
                ProviderId = request.ProviderId,
                AppointmentDateTime = request.AppointmentDateTime.ToUniversalTime(),
                ReasonForAppointment = request.ReasonForAppointment,
                Status = AppointmentStatus.Pending
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync(cancellationToken);

            //send email
            await _notificationService.SendAppointmentConfirmationAsync(appointment);

            response.isSuccess = true;
            response.Message = "Appointment Booked Successfully";
            response.ResponseCode = "00";
            response.Data = appointment.Id;
            return response;
        }
    }
}
