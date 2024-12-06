using MediatR;
using AppointmentScheduler.Infrastructure.Data;
using AppointmentScheduler.Domain.Models;
using AppointmentScheduler.Domain.DTOs;

namespace AppointmentScheduler.Application.Appointments.Commands.Handlers
{
    public class CreateProviderCommandHandler : IRequestHandler<CreateProviderCommand, ApiResponse<Guid>>
    {
        private readonly SchedulerDbContext _context;

        public CreateProviderCommandHandler(SchedulerDbContext context)
        {
            _context = context;
        }
        
        public async Task<ApiResponse<Guid>> Handle(CreateProviderCommand request, CancellationToken cancellationToken)
        {
            var provider = new Provider
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Specialty = request.Specialty,
                Email = request.Email,
                Phone = request.Phone,
            };

            // Add the provider
            _context.Providers.Add(provider);
            await _context.SaveChangesAsync(cancellationToken);

            return new ApiResponse<Guid>
            {
                isSuccess = true,
                ResponseCode = "00",
                Message = $"Provider {request.Name} has been created successfully.",
                Data = provider.Id
            };
        }
    }
}
