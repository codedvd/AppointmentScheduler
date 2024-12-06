using AppointmentScheduler.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Domain.DTOs
{
    public record AppointmentDto(
       Guid Id,
       string? PatientName,
       string? DateOfBirth,
       string? NationalId,
       string? PatientEmail,
       Guid ProviderId,
       string? ProviderName,
       DateTime AppointmentDateTime,
       string? ReasonForAppointment,
       AppointmentStatus Status
   );
}
