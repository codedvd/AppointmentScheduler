using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Application.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    namespace AppointmentScheduling.Exceptions
    {
        [Serializable]
        public class OverlappingAppointmentException : Exception
        {
            public int? ConflictingAppointmentId { get; }
            public DateTime? ConflictingAppointmentTime { get; }

            public OverlappingAppointmentException()
                : base("An appointment already exists in the selected time slot.")
            {
            }

            public OverlappingAppointmentException(string message)
                : base(message)
            {
            }

            public OverlappingAppointmentException(
                string message,
                int? conflictingAppointmentId = null,
                DateTime? conflictingAppointmentTime = null)
                : base(message)
            {
                ConflictingAppointmentId = conflictingAppointmentId;
                ConflictingAppointmentTime = conflictingAppointmentTime;
            }

            public OverlappingAppointmentException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            protected OverlappingAppointmentException(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
                ConflictingAppointmentId = (int?)info.GetValue(nameof(ConflictingAppointmentId), typeof(int?));
                ConflictingAppointmentTime = (DateTime?)info.GetValue(nameof(ConflictingAppointmentTime), typeof(DateTime?));
            }

            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);

                info.AddValue(nameof(ConflictingAppointmentId), ConflictingAppointmentId);
                info.AddValue(nameof(ConflictingAppointmentTime), ConflictingAppointmentTime);
            }
        }
    }
}
