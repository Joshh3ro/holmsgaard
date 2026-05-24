using Holmsgaard.ApiService.Application.Commands;
using Holmsgaard.ApiService.Application.Services;
using Holmsgaard.ApiService.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Holmsgaard.ApiService.Controllers;

[ApiController]
[Route("api/time-registrations")]
public sealed class TimeRegistrationsController(TimeCalculationService timeCalculationService) : ControllerBase
{
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
}
