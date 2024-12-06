using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Domain.DTOs
{
    public record AppointmentReportDto(
        int TotalAppointments,
        int CancelledAppointments,
        int CompletedAppointments,
        decimal CancellationRate,
        Dictionary<string, int> AppointmentsByProvider
    );
}
