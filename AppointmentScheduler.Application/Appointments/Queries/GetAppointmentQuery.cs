using AppointmentScheduler.Domain.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Application.Appointments.Queries
{
    public class GetAppointmentQuery : IRequest<ApiResponse<AppointmentDto>>
    {
        public Guid AppointmentId { get; set; }

        public GetAppointmentQuery(Guid appointmentId)
        {
            AppointmentId = appointmentId;
        }
    }
}
