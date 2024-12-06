using MediatR;
using Microsoft.EntityFrameworkCore;
using AppointmentScheduler.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;
using AppointmentScheduler.Application.Appointments.Queries;
using AppointmentScheduler.Domain.DTOs;
using AppointmentScheduler.Domain.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

public class GetAppointmentReportQueryHandler : IRequestHandler<GetAppointmentReportQuery, FetchAppointmentReportDto>
{
    private readonly SchedulerDbContext _context;

    public GetAppointmentReportQueryHandler(SchedulerDbContext context)
    {
        _context = context;
    }

    public async Task<FetchAppointmentReportDto> Handle(GetAppointmentReportQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Appointments.AsQueryable();

        if (request.StartDate.HasValue && request.EndDate.HasValue)
        {
            query = query.Where(a => a.AppointmentDateTime >= request.StartDate.Value.ToUniversalTime() &&
                    a.AppointmentDateTime <= request.EndDate.Value.ToUniversalTime());
        }

        var totalAppointments = await query.CountAsync(cancellationToken);
        var totalCancellations = await query.CountAsync(a => a.Status == AppointmentStatus.Cancelled, cancellationToken);
        var completedAppointments = await query.CountAsync(a => a.Status == AppointmentStatus.Completed, cancellationToken);
        var pendingAppointments = await query.CountAsync(a => a.Status == AppointmentStatus.Pending, cancellationToken);

        return new FetchAppointmentReportDto
        {
            TotalAppointments = totalAppointments,
            TotalCancellations = totalCancellations,
            CompletedAppointments = completedAppointments,
            PendingAppointments = pendingAppointments,
        };
    }
}
