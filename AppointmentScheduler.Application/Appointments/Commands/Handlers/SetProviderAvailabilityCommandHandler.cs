using MediatR;
using AppointmentScheduler.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AppointmentScheduler.Domain.Models;
using AppointmentScheduler.Domain.DTOs;

namespace AppointmentScheduler.Application.Appointments.Commands.Handlers
{
    public class SetProviderAvailabilityCommandHandler : IRequestHandler<SetProviderAvailabilityCommand, ApiResponse<Unit>>
    {
        private readonly SchedulerDbContext _context;

        public SetProviderAvailabilityCommandHandler(SchedulerDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<Unit>> Handle(SetProviderAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<Unit>();
            var providerExists = await _context.Providers.AnyAsync(
                p => p.Id == request.ProviderId,
                cancellationToken
            );

            if (!providerExists)
            {
                response.isSuccess = false;
                response.ResponseCode = "06";
                response.Message = "Provider not found.";
                return response;
            }

            var providerAvailability = new ProviderAvailability
            {
                ProviderId = request.ProviderId,
                DayOfWeek = request.DayOfWeek,
                StartTime = request.StartTime.TimeOfDay,
                EndTime = request.EndTime.TimeOfDay
            };

            _context.ProviderAvailabilities.Add(providerAvailability);
            await _context.SaveChangesAsync(cancellationToken);

            response.isSuccess = true;
            response.Message = "Avaliability Setup successfully";
            response.ResponseCode = "00";
            response.Data = Unit.Value;
            return response;
        }
    }
}
