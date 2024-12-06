using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Domain.Models
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string DateOfBirth { get; set; }
        public string NationalId { get; set; }
        public string PatientEmail { get; set; }
        public Guid ProviderId { get; set; }
        public Provider Provider { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public TimeSpan Duration { get; set; }
        public string? ReasonForAppointment { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
