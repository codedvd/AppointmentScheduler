using AppointmentScheduler.Domain.DTOs;
using AppointmentScheduler.Domain.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Infrastructure.Services
{
    public class EmailNotificationService : INotificationService
    {
        private readonly SmtpClient _smtpClient;
        private readonly EmailSettings _emailSettings;

        public EmailNotificationService(
            SmtpClient smtpClient,
            IOptions<EmailSettings> emailSettings)
        {
            _smtpClient = smtpClient;
            _emailSettings = emailSettings.Value;
        }

        public async Task SendAppointmentConfirmationAsync(Appointment appointment)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail),
                Subject = "Appointment Confirmation",
                Body = $"Dear {appointment.FullName}, " +
                       $"Your appointment is confirmed for {appointment.AppointmentDateTime}"
            };
            mailMessage.To.Add(appointment.PatientEmail);

            await _smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendAppointmentReminderAsync(Appointment appointment)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail),
                Subject = "Appointment Reminder",
                Body = $"Reminder: You have an appointment on {appointment.AppointmentDateTime}"
            };
            mailMessage.To.Add(appointment.PatientEmail);

            await _smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendAppointmentCompletionAsync(Appointment appointment)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail),
                Subject = "Appointment Completed",
                Body = $"Notice: Your appointment is completed and closed on {appointment.AppointmentDateTime}"
            };
            mailMessage.To.Add(appointment.PatientEmail);

            await _smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendAppointmentCancellationAsync(Appointment appointment)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail),
                Subject = "Appointment Cancellation",
                Body = $"Your appointment on {appointment.AppointmentDateTime} has been cancelled."
            };
            mailMessage.To.Add(appointment.PatientEmail);

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
