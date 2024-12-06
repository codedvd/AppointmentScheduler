using AppointmentScheduler.Application.Appointments.Queries;
using AppointmentScheduler.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAppointmentReportMetrics")]
        public async Task<ActionResult<AppointmentReportDto>> GetAppointmentReport(
        [FromQuery] GetAppointmentReportQuery query)
        {
            var report = await _mediator.Send(query);
            return Ok(report);
        }
    }
}
