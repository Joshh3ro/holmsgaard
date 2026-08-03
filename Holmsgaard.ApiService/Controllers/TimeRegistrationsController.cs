using Holmsgaard.ApiService.Application.Commands;
using Holmsgaard.ApiService.Application.Services;
using Holmsgaard.ApiService.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Holmsgaard.ApiService.Controllers;

[ApiController]
[Route("api/time-registrations")]
public sealed class TimeRegistrationsController(TimeCalculationService timeCalculationService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyCollection<TimeRegistrationDto>> GetTimeRegistrations([FromQuery] Guid? employeeId = null)
    {
        return Ok(timeCalculationService.GetTimeRegistrations(employeeId));
    }

    [HttpPost]
    public ActionResult<TimeRegistrationDto> RegisterTime(RegisterTimeRequest request)
    {
        try
        {
            var registration = timeCalculationService.RegisterTime(new RegisterEmployeeTimeCommand(
                request.EmployeeId,
                request.ActivityId,
                request.WorkDate,
                request.Hours,
                request.Note));

            return Created($"/api/time-registrations/{registration.Id}", registration);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public ActionResult<TimeRegistrationDto> UpdateTimeRegistration(Guid id, UpdateTimeRegistrationRequest request)
    {
        try
        {
            var registration = timeCalculationService.UpdateTimeRegistration(new UpdateTimeRegistrationCommand(
                id,
                request.EmployeeId,
                request.ActivityId,
                request.WorkDate,
                request.Hours,
                request.Note));

            return registration is null ? NotFound() : Ok(registration);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteTimeRegistration(Guid id)
    {
        return timeCalculationService.DeleteTimeRegistration(id) ? NoContent() : NotFound();
    }
}
