using Holmsgaard.ApiService.Application.Commands;
using Holmsgaard.ApiService.Application.Queries;
using Holmsgaard.ApiService.Application.Services;
using Holmsgaard.ApiService.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Holmsgaard.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class EmployeesController(EmployeeService employeeService, TimeCalculationService timeCalculationService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyCollection<EmployeeDto>> GetEmployees([FromQuery] bool includeInactive = false)
    {
        return Ok(employeeService.GetEmployees(new GetEmployeesQuery(includeInactive)));
    }

    [HttpGet("{id:guid}")]
    public ActionResult<EmployeeDto> GetEmployee(Guid id)
    {
        var employee = employeeService.GetEmployee(new GetEmployeeByIdQuery(id));
        return employee is null ? NotFound() : Ok(employee);
    }

    [HttpGet("{employeeId:guid}/time-summary")]
    public ActionResult<EmployeeTimeSummaryDto> GetTimeSummary(Guid employeeId)
    {
        var summary = timeCalculationService.GetEmployeeTimeSummary(new GetEmployeeTimeSummaryQuery(employeeId));
        return summary is null ? NotFound() : Ok(summary);
    }

    [HttpPost]
    public ActionResult<EmployeeDto> CreateEmployee(CreateEmployeeRequest request)
    {
        try
        {
            var employee = employeeService.CreateEmployee(new CreateEmployeeCommand(request.FullName, request.Email, request.HourlyRate));
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public ActionResult<EmployeeDto> UpdateEmployee(Guid id, UpdateEmployeeRequest request)
    {
        try
        {
            var employee = employeeService.UpdateEmployee(new UpdateEmployeeCommand(id, request.FullName, request.Email, request.HourlyRate, request.IsActive));
            return employee is null ? NotFound() : Ok(employee);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteEmployee(Guid id)
    {
        return employeeService.DeleteEmployee(id) ? NoContent() : NotFound();
    }
}
