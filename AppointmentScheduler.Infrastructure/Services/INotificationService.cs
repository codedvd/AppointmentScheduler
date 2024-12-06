using AppointmentScheduler.Domain.Models;

namespace AppointmentScheduler.Infrastructure.Services
{
    public interface INotificationService
    {
        Task SendAppointmentConfirmationAsync(Appointment appointment);
        Task SendAppointmentReminderAsync(Appointment appointment);
        Task SendAppointmentCancellationAsync(Appointment appointment);
        Task SendAppointmentCompletionAsync(Appointment appointment);
    }
}
