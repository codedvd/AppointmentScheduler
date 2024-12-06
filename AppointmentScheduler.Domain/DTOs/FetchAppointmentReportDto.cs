using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Domain.DTOs
{
    public class FetchAppointmentReportDto
    {
        public int TotalAppointments { get; set; }
        public int TotalCancellations { get; set; }
        public int CompletedAppointments { get; set; }
        public int PendingAppointments { get; set; }
    }
}
