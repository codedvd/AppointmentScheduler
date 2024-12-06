using AppointmentScheduler.Domain.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Application.Appointments.Queries
{
    public record SearchAppointmentsQuery(
        DateTime? StartDate = null,
        DateTime? EndDate = null,
        Guid? ProviderId = null,
        string? PatientName = null
    ) : IRequest<ApiResponse<IEnumerable<FetchAppointmentDto>>>;
}
