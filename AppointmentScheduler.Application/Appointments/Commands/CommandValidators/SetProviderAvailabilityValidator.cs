using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Application.Appointments.Commands.CommandValidators
{
    public class SetProviderAvailabilityValidator : AbstractValidator<SetProviderAvailabilityCommand>
    {
        public SetProviderAvailabilityValidator()
        {
            RuleFor(x => x.ProviderId)
                .NotNull().NotEmpty().WithMessage("Provider Id is required");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time needs to be specified!");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time needs to be specified!");

            RuleFor(x => x.DayOfWeek)
                .NotEmpty().WithErrorCode("06").WithMessage("Please specify day of the week");
        }
    }
}
