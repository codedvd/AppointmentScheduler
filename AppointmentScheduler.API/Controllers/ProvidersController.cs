using AppointmentScheduler.Application.Appointments.Commands;
using AppointmentScheduler.Application.Appointments.Queries;
using AppointmentScheduler.Domain.DTOs;
using AppointmentScheduler.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProvidersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly SchedulerDbContext _context;

        public ProvidersController(IMediator mediator, SchedulerDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [HttpPost("CreateAvailability")]
        public async Task<IActionResult> SetProviderAvailability(
            [FromBody] SetProviderAvailabilityCommand command)
        {
            var response = await _mediator.Send(command);
            return response.ResponseCode == "00" ? Ok(response) : BadRequest(response);
        }

        [HttpGet("appointments")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetProviderAppointments(
        [FromQuery] GetProviderAppointmentsQuery query)
        {
            var appointments = await _mediator.Send(query);
            return Ok(appointments);
        }

        [HttpPost("createProvider")]
        public async Task<IActionResult> CreateProvider([FromBody] CreateProviderCommand command)
        {
            if (command == null)
            {
                return BadRequest("Provider details are required.");
            }

            var providerId = await _mediator.Send(command);
            return providerId.ResponseCode == "00" ? Ok(providerId) : BadRequest(providerId);
        }

        [HttpGet("getProviderById")]
        public async Task<IActionResult> GetProvider([FromQuery]GetProviderQuery id)
        {
            var provider = await _mediator.Send(id);
            return !provider.isSuccess ? NotFound(provider) : Ok(provider);
        }

        [HttpGet("getAllProviders")]
        public async Task<IActionResult> GetAllProviders()
        {
            var providers = await _mediator.Send(new GetAllProvidersQuery());
            return !providers.isSuccess ? NotFound(providers) : Ok(providers);
        }
    }
}
