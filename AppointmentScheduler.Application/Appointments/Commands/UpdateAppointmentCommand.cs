using AppointmentScheduler.Domain.DTOs;
using AppointmentScheduler.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppointmentScheduler.Application.Appointments.Commands
{
    public record UpdateAppointmentCommand(
        Guid AppointmentId,
        Guid ProviderId
    ) : IRequest<ApiResponse<bool>>
    {
        public AppointmentStatus AppointmentStatus { get; init; }
    }
}
