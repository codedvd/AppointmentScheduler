using AppointmentScheduler.Domain.DTOs;
using MediatR;

namespace AppointmentScheduler.Application.Appointments.Queries
{
    public class GetProviderQuery : IRequest<ApiResponse<ProviderDto>>
    {
        public Guid ProviderId { get; set; }
        public GetProviderQuery() { }
        public GetProviderQuery(Guid providerId)
        {
            ProviderId = providerId;
        }
    }
}
