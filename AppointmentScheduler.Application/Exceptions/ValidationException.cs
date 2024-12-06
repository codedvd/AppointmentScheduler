using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public List<string> Errors { get; }

        public ValidationException(IEnumerable<string> errors) : base("One or more validation failures occurred.")
        {
            Errors = errors.ToList();
        }

        public ValidationException(string message) : base(message)
        {
            Errors = new List<string> { message };
        }
    }
}
