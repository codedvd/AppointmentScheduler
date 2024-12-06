using AppointmentScheduler.Application.Appointments.Commands;
using AppointmentScheduler.Application.Appointments.Queries;
using AppointmentScheduler.Application.Appointments.Queries.Handlers;
using AppointmentScheduler.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduler.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("BookAppointment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> BookAppointment([FromBody] BookAppointmentCommand command)
        {
            try
            {
                var appointmentId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetAppointment), new { id = appointmentId }, appointmentId);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Data);
            }
        }

        [HttpGet("GetAppointment/{id}")]
        public async Task<ActionResult<AppointmentDto>> GetAppointment(Guid id)
        {
            var query = new GetAppointmentQuery(id);
            var appointment = await _mediator.Send(query);

            return appointment.ResponseCode == "00"
                ? Ok(appointment)
                : NotFound(appointment);
        }

        [HttpGet("SearchForAppointment")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> SearchAppointments(
        [FromQuery] SearchAppointmentsQuery query)
        {
            var appointments = await _mediator.Send(query);
            return Ok(appointments);
        }

        [HttpPost("UpdateAppointment")]
        public async Task<IActionResult> UpdateAppointment([FromBody]UpdateAppointmentCommand update)
        {
            var result = await _mediator.Send(update);
            return Ok(result);
        }
    }
}
