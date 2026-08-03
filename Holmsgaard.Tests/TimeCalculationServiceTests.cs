using Holmsgaard.ApiService.Application.Commands;
using Holmsgaard.ApiService.Application.Queries;
using Holmsgaard.ApiService.Application.Services;
using Holmsgaard.ApiService.Infrastructure.InMemory;

namespace Holmsgaard.Tests;

public class TimeCalculationServiceTests
{
    private static readonly Guid DemoEmployeeId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Guid DemoActivityId = Guid.Parse("33333333-3333-3333-3333-333333333333");

    [Fact]
    public void RegisterTimeAddsHoursToEmployeeSummary()
    {
        var service = CreateService();

        service.RegisterTime(new RegisterEmployeeTimeCommand(DemoEmployeeId, DemoActivityId, new DateOnly(2026, 5, 23), 7.5m, "Normal arbejdsdag"));

        var summary = service.GetEmployeeTimeSummary(new GetEmployeeTimeSummaryQuery(DemoEmployeeId));

        Assert.NotNull(summary);
        Assert.Equal(7.5m, summary.TotalHours);
        Assert.Equal(2625m, summary.TotalCost);
    }

    [Fact]
    public void MultipleRegistrationsAreSummed()
    {
        var service = CreateService();

        service.RegisterTime(new RegisterEmployeeTimeCommand(DemoEmployeeId, DemoActivityId, new DateOnly(2026, 5, 23), 2m, "Formiddag"));
        service.RegisterTime(new RegisterEmployeeTimeCommand(DemoEmployeeId, DemoActivityId, new DateOnly(2026, 5, 23), 3m, "Eftermiddag"));

        var summary = service.GetEmployeeTimeSummary(new GetEmployeeTimeSummaryQuery(DemoEmployeeId));

        Assert.NotNull(summary);
        Assert.Equal(5m, summary.TotalHours);
    }

    [Fact]
    public void GetTimeRegistrationsCanFilterByEmployee()
    {
        var service = CreateService();

        service.RegisterTime(new RegisterEmployeeTimeCommand(DemoEmployeeId, DemoActivityId, new DateOnly(2026, 5, 23), 2m, "Formiddag"));
        service.RegisterTime(new RegisterEmployeeTimeCommand(DemoEmployeeId, DemoActivityId, new DateOnly(2026, 5, 24), 3m, "Eftermiddag"));

        var registrations = service.GetTimeRegistrations(DemoEmployeeId);

        Assert.Equal(2, registrations.Count);
        Assert.All(registrations, registration => Assert.Equal(DemoEmployeeId, registration.EmployeeId));
        Assert.Equal(new DateOnly(2026, 5, 24), registrations.First().WorkDate);
    }

    [Fact]
    public void UpdateTimeRegistrationChangesExistingRegistration()
    {
        var service = CreateService();
        var registration = service.RegisterTime(new RegisterEmployeeTimeCommand(DemoEmployeeId, DemoActivityId, new DateOnly(2026, 5, 23), 2m, "Formiddag"));

        var updated = service.UpdateTimeRegistration(new UpdateTimeRegistrationCommand(
            registration.Id,
            DemoEmployeeId,
            DemoActivityId,
            new DateOnly(2026, 5, 24),
            4.5m,
            "Rettet note"));

        Assert.NotNull(updated);
        Assert.Equal(new DateOnly(2026, 5, 24), updated.WorkDate);
        Assert.Equal(4.5m, updated.Hours);
        Assert.Equal("Rettet note", updated.Note);
    }

    [Fact]
    public void DeleteTimeRegistrationRemovesExistingRegistration()
    {
        var service = CreateService();
        var registration = service.RegisterTime(new RegisterEmployeeTimeCommand(DemoEmployeeId, DemoActivityId, new DateOnly(2026, 5, 23), 2m, "Formiddag"));

        var deleted = service.DeleteTimeRegistration(registration.Id);
        var registrations = service.GetTimeRegistrations(DemoEmployeeId);

        Assert.True(deleted);
        Assert.Empty(registrations);
    }

    [Fact]
    public void ZeroHoursAreAllowed()
    {
        var service = CreateService();

        var registration = service.RegisterTime(new RegisterEmployeeTimeCommand(DemoEmployeeId, DemoActivityId, new DateOnly(2026, 5, 23), 0m, "Ingen timer"));

        Assert.Equal(0m, registration.Hours);
    }

    [Fact]
    public void UnknownEmployeeThrows()
    {
        var service = CreateService();

        Assert.Throws<InvalidOperationException>(() =>
            service.RegisterTime(new RegisterEmployeeTimeCommand(Guid.NewGuid(), DemoActivityId, new DateOnly(2026, 5, 23), 1m, "Ukendt medarbejder")));
    }

    [Fact]
    public void UnknownActivityThrows()
    {
        var service = CreateService();

        Assert.Throws<InvalidOperationException>(() =>
            service.RegisterTime(new RegisterEmployeeTimeCommand(DemoEmployeeId, Guid.NewGuid(), new DateOnly(2026, 5, 23), 1m, "Ukendt aktivitet")));
    }

    private static TimeCalculationService CreateService()
    {
        return new TimeCalculationService(
            new InMemoryTimeRegistrationRepository(),
            new InMemoryEmployeeRepository(),
            new InMemoryActivityRepository());
    }
}
