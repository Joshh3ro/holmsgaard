using Holmsgaard.ApiService.Application.Commands;
using Holmsgaard.ApiService.Application.Services;
using Holmsgaard.ApiService.Controllers;
using Holmsgaard.ApiService.Contracts.Dto;
using Holmsgaard.ApiService.Infrastructure.InMemory;
using Holmsgaard.ApiService.Domain.Entities;
using Holmsgaard.ApiService.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Holmsgaard.Tests;

public class ControllerTests
{
    [Fact]
    public void GetCustomerReturnsOkWhenCustomerExists()
    {
        var service = new CustomerService(new InMemoryCustomerRepository());
        var customer = service.CreateCustomer(new CreateCustomerCommand("Controller Kunde", "controller@example.com", "12345678"));
        var controller = new CustomersController(service);

        var response = controller.GetCustomer(customer.Id);

        Assert.IsType<OkObjectResult>(response.Result);
    }

    [Fact]
    public void GetCustomerReturnsNotFoundWhenCustomerDoesNotExist()
    {
        var controller = new CustomersController(new CustomerService(new InMemoryCustomerRepository()));

        var response = controller.GetCustomer(Guid.NewGuid());

        Assert.IsType<NotFoundResult>(response.Result);
    }

    [Fact]
    public void CreateCustomerReturnsCreated()
    {
        var controller = new CustomersController(new CustomerService(new InMemoryCustomerRepository()));

        var response = controller.CreateCustomer(new CreateCustomerRequest("Created Kunde", "created@example.com", "12345678"));

        Assert.IsType<CreatedAtActionResult>(response.Result);
    }

    [Fact]
    public void CreateCustomerReturnsBadRequestForInvalidRequest()
    {
        var controller = new CustomersController(new CustomerService(new InMemoryCustomerRepository()));

        var response = controller.CreateCustomer(new CreateCustomerRequest("", "created@example.com", "12345678"));

        Assert.IsType<BadRequestObjectResult>(response.Result);
    }

    [Fact]
    public void DeleteCustomerReturnsNoContent()
    {
        var service = new CustomerService(new InMemoryCustomerRepository());
        var customer = service.CreateCustomer(new CreateCustomerCommand("Delete Kunde", "delete@example.com", "12345678"));
        var controller = new CustomersController(service);

        var response = controller.DeleteCustomer(customer.Id);

        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public void AuthRegisterCreatesEmployeeAndReturnsOk()
    {
        var repo = new InMemoryEmployeeRepository();
        var controller = CreateAuthController(repo);

        var response = controller.Register(new RegisterRequest("Test User", "test@hgaps.dk", "hemmeligt123"));

        var result = Assert.IsType<OkObjectResult>(response.Result);
        var authResponse = Assert.IsType<AuthResponse>(result.Value);
        Assert.Equal("Bearer", authResponse.TokenType);
        Assert.NotEmpty(authResponse.Token);

        var employee = Assert.Single(repo.GetAll(), employee => employee.Email == "test@hgaps.dk");
        Assert.NotEqual("hemmeligt123", employee.PasswordHash);
    }

    [Fact]
    public void AuthLoginReturnsJwtForCorrectPassword()
    {
        var repo = new InMemoryEmployeeRepository();
        var controller = CreateAuthController(repo);
        controller.Register(new RegisterRequest("Login User", "login@hgaps.dk", "hemmeligt123"));

        var response = controller.Login(new LoginRequest("login@hgaps.dk", "hemmeligt123"));

        var result = Assert.IsType<OkObjectResult>(response.Result);
        var authResponse = Assert.IsType<AuthResponse>(result.Value);
        Assert.Equal(3, authResponse.Token.Split('.').Length);
    }

    [Fact]
    public void AuthLoginRejectsWrongPassword()
    {
        var repo = new InMemoryEmployeeRepository();
        var controller = CreateAuthController(repo);
        controller.Register(new RegisterRequest("Login User", "login@hgaps.dk", "hemmeligt123"));

        var response = controller.Login(new LoginRequest("login@hgaps.dk", "forkert123"));

        Assert.IsType<UnauthorizedObjectResult>(response.Result);
    }

    private static AuthController CreateAuthController(InMemoryEmployeeRepository repository)
    {
        var options = Options.Create(new JwtOptions
        {
            Key = AuthIntegrationTests.TestSigningKey
        });

        return new AuthController(
            repository,
            new PasswordHasher<Employee>(),
            new JwtTokenService(options));
    }
}
