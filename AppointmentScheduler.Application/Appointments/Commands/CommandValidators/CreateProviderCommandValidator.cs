using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Application.Appointments.Commands.CommandValidators
{
    public class CreateProviderCommandValidator : AbstractValidator<CreateProviderCommand>
    { 
        public CreateProviderCommandValidator()
        {
            RuleFor(a => a.Email)
                .NotEmpty().WithMessage("Provider Email is required")
                .MaximumLength(100).WithMessage("Provider Email cannot exceed 100 characters");

            RuleFor(a => a.Name)
                .NotEmpty().WithMessage("Provder name is required")
                .MaximumLength(100).WithMessage("Proider name cannot exceed 100 characters");

            RuleFor(a => a.Phone)
                .NotEmpty();

            RuleFor(a => a.Specialty)
                .NotEmpty().WithMessage("Please specify your specialty!");
        }
    }
}
