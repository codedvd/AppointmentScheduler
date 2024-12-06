using AppointmentScheduler.Domain.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Application.Appointments.Commands
{
    public record SetProviderAvailabilityCommand(
        Guid ProviderId,
        DayOfWeek DayOfWeek,
        DateTime StartTime,
        DateTime EndTime
    ) : IRequest<ApiResponse<Unit>>;
}
