using Holmsgaard.ApiService;
using Holmsgaard.ApiService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add DbContext with SQL Server. Use the connection string key defined in appsettings.json
// (HOLMSGAARD_CONTEXT). Fall back to localhost if the key is missing.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("HOLMSGAARD_CONTEXT")
        ?? "Server=localhost;Database=Holmsgaard;Trusted_Connection=True;TrustServerCertificate=True"));

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHolmsgaardBackend();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", () => "Holmsgaard API service is running. Navigate to /openapi/v1.json to inspect the API.");

app.MapControllers();

app.MapDefaultEndpoints();

app.Run();
