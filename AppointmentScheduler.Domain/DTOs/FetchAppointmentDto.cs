using AppointmentScheduler.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Domain.DTOs
{
    public class FetchAppointmentDto
    {
        public string PatientName { get; set; }
        public string PatientEmail { get; set; }
        public TimeSpan Duration { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string ReasonForAppointment { get; set; }
    }
}
