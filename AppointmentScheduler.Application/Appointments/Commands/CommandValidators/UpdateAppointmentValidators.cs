using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Application.Appointments.Commands.CommandValidators
{
    public class UpdateAppointmentValidators : AbstractValidator<UpdateAppointmentCommand>
    {
        public UpdateAppointmentValidators()
        {
            RuleFor(a => a.AppointmentId)
                .NotEmpty().WithMessage("Provider Id is required");

            RuleFor(b => b.ProviderId)
                .NotEmpty().WithErrorCode("06").WithMessage("Provider Id is required");
        }
    }
}
