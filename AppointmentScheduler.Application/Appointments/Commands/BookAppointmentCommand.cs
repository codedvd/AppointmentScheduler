using AppointmentScheduler.Domain.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Application.Appointments.Commands
{
    public record BookAppointmentCommand(
        string PatientName,
        string DateOfBirth,
        string NationalId,
        string PatientEmail,
        Guid ProviderId,
        DateTime AppointmentDateTime,
        string? ReasonForAppointment = null
    ) : IRequest<ApiResponse<Guid>>;
}
