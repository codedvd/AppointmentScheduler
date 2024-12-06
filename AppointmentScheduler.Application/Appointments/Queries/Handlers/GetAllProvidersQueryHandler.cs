using AppointmentScheduler.Domain.DTOs;
using AppointmentScheduler.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Application.Appointments.Queries.Handlers
{
    public class GetAllProvidersQueryHandler : IRequestHandler<GetAllProvidersQuery, ApiResponse<List<ProviderDto>>>
    {
        private readonly SchedulerDbContext _context;

        public GetAllProvidersQueryHandler(SchedulerDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<ProviderDto>>> Handle(GetAllProvidersQuery request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<List<ProviderDto>>();
            var providers = await _context.Providers
                .AsNoTracking() 
                .ToListAsync(cancellationToken);

            if (providers == null || !providers.Any())
            {
                response.isSuccess = false;
                response.ResponseCode = "06";
                response.Message = "No providers found.";
                return response;
            }

            var providerDtos = providers.Select(p => new ProviderDto
            {
                Name = p.Name,
                Specialty = p.Specialty,
                Email = p.Email,
                Phone = p.Phone
            }).ToList();

            response.isSuccess = true;
            response.ResponseCode = "06";
            response.Message = "Returned all providers suucessfully.";
            response.Data = providerDtos;
            return response;
        }
    }

}
