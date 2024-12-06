using AppointmentScheduler.Domain.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Application.Appointments.Queries
{
    public record GetAppointmentReportQuery(
        DateTime? StartDate = null,
        DateTime? EndDate = null
    ) : IRequest<FetchAppointmentReportDto>;
}
