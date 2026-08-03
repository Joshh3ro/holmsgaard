using System.Net.Http.Headers;
using Holmsgaard.ApiService;
using Holmsgaard.ApiService.Domain.Entities;
using Holmsgaard.ApiService.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Holmsgaard.Tests;

public sealed class AuthIntegrationTests
{
    public const string TestSigningKey = "test-only-signing-key-with-at-least-32-characters";

    [Fact]
    public async Task ProtectedEndpointRequiresAValidJwt()
    {
        await using var factory = new WebApplicationFactory<ApiAssemblyMarker>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseSetting("Jwt:Key", TestSigningKey);
            });

        using var client = factory.CreateClient();

        var responseWithoutToken = await client.GetAsync("/api/auth/me");
        Assert.Equal(HttpStatusCode.Unauthorized, responseWithoutToken.StatusCode);

        var employee = new Employee(
            Guid.NewGuid(),
            "Integration User",
            "integration@hgaps.dk",
            0m);
        var tokenService = factory.Services.GetRequiredService<JwtTokenService>();
        var authResponse = tokenService.CreateToken(employee);
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(authResponse.TokenType, authResponse.Token);

        var responseWithToken = await client.GetAsync("/api/auth/me");
        var challenge = string.Join(", ", responseWithToken.Headers.WwwAuthenticate);
        Assert.True(
            responseWithToken.IsSuccessStatusCode,
            $"Expected a successful response, but received {responseWithToken.StatusCode}. Challenge: {challenge}");
    }
}
