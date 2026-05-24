namespace Holmsgaard.ApiService.Domain.Entities;

public sealed class TimeRegistration
{
    public Guid Id { get; }
    public Guid EmployeeId { get; }
    public Guid ActivityId { get; }
    public DateOnly WorkDate { get; private set; }
    public decimal Hours { get; private set; }
    public string Note { get; private set; }

    public TimeRegistration(Guid id, Guid employeeId, Guid activityId, DateOnly workDate, decimal hours, string note)
    {
        if (employeeId == Guid.Empty)
        {
            throw new ArgumentException("Employee id is required.", nameof(employeeId));
        }

        if (activityId == Guid.Empty)
        {
            throw new ArgumentException("Activity id is required.", nameof(activityId));
        }

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        EmployeeId = employeeId;
        ActivityId = activityId;
        WorkDate = workDate;
        Note = note.Trim();
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
