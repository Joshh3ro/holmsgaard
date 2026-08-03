using Holmsgaard.ApiService.Domain.Entities;
using Holmsgaard.ApiService.Domain.Enums;

public sealed class TimeRegistration
{
    public Guid Id { get; }
    public Guid EmployeeId { get; private set; }
    public Guid ActivityId { get; private set; }
    public DateOnly WorkDate { get; private set; }
    public decimal Hours { get; private set; }
    public string Note { get; private set; } = string.Empty;
    public RegistrationType Type { get; private set; }

    public TimeRegistration(Guid id, Guid employeeId, Guid activityId, DateOnly workDate, decimal hours, string note, RegistrationType type = RegistrationType.Normal)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Update(employeeId, activityId, workDate, hours, note, type);
    }

    public void Update(Guid employeeId, Guid activityId, DateOnly workDate, decimal hours, string note, RegistrationType type = RegistrationType.Normal)
    {
        if (employeeId == Guid.Empty)
        {
            throw new ArgumentException("Employee id is required.", nameof(employeeId));
        }

        if (activityId == Guid.Empty)
        {
            throw new ArgumentException("Activity id is required.", nameof(activityId));
        }

        EmployeeId = employeeId;
        ActivityId = activityId;
        WorkDate = workDate;
        Note = (note ?? string.Empty).Trim();
        Type = type;
        RegisterHours(hours);
    }

    public void RegisterHours(decimal hours)
    {
        if (hours < 0 || hours > 24)
        {
            throw new ArgumentOutOfRangeException(nameof(hours), "Hours must be between 0 and 24.");
        }

        Hours = hours;
    }
}
