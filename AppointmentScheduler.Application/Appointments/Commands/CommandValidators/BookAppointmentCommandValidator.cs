using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Application.Appointments.Commands.CommandValidators
{
    public class BookAppointmentCommandValidator : AbstractValidator<BookAppointmentCommand>
    {
        public BookAppointmentCommandValidator()
        {
            RuleFor(x => x.PatientName)
                .NotEmpty().WithMessage("Patient name is required")
                .MaximumLength(100).WithMessage("Patient name cannot exceed 100 characters");

            RuleFor(x => x.NationalId)
                .NotEmpty().WithMessage("National ID is required")
                .Length(9, 12).WithMessage("National ID must be between 9 and 12 characters");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth is required")
                .Must(BeAValidDate).WithMessage("Invalid Date of Birth");

            RuleFor(x => x.AppointmentDateTime)
                .NotEmpty().WithMessage("Appointment date and time are required")
                .Must(BeAFutureDate).WithMessage("Appointment must be in the future");
        }

        private bool BeAValidDate(string dateOfBirth)
        {
            return DateTime.Parse(dateOfBirth) < DateTime.Now && DateTime.Parse(dateOfBirth).AddYears(150) > DateTime.Now;
        }

        private bool BeAFutureDate(DateTime appointmentDateTime)
        {
            return appointmentDateTime > DateTime.Now;
        }
    }
}
