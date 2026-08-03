using Holmsgaard.ApiService.Application.Commands;
using Holmsgaard.ApiService.Application.Queries;
using Holmsgaard.ApiService.Application.Services;
using Holmsgaard.ApiService.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Holmsgaard.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CustomersController(CustomerService customerService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyCollection<CustomerDto>> GetCustomers([FromQuery] bool includeInactive = false)
    {
        return Ok(customerService.GetCustomers(new GetCustomersQuery(includeInactive)));
    }

    [HttpGet("{id:guid}")]
    public ActionResult<CustomerDto> GetCustomer(Guid id)
    {
        var customer = customerService.GetCustomer(new GetCustomerByIdQuery(id));
        return customer is null ? NotFound() : Ok(customer);
    }

    [HttpPost]
    public ActionResult<CustomerDto> CreateCustomer(CreateCustomerRequest request)
    {
        try
        {
            var customer = customerService.CreateCustomer(new CreateCustomerCommand(request.Name, request.Email, request.Phone));
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public ActionResult<CustomerDto> UpdateCustomer(Guid id, UpdateCustomerRequest request)
    {
        try
        {
            var customer = customerService.UpdateCustomer(new UpdateCustomerCommand(id, request.Name, request.Email, request.Phone));
            return customer is null ? NotFound() : Ok(customer);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteCustomer(Guid id)
    {
        return customerService.DeleteCustomer(id) ? NoContent() : NotFound();
    }
}
