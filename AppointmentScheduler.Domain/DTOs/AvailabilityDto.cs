using AppointmentScheduler.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Domain.DTOs
{
    public class AvailabilityDto
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

    public class ProviderDto
    {
        public string Name { get; set; }
        public string Specialty { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<FetchAppointmentDto> Appointments { get; set; } = new List<FetchAppointmentDto>();
        public List<AvailabilityDto> Availabilities { get; set; } = new List<AvailabilityDto>();
    }

}

