namespace Holmsgaard.ApiService.Domain.Entities;

public sealed class Employee
{
    public Guid Id { get; }
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public decimal HourlyRate { get; private set; }
    public bool IsActive { get; private set; }

    public Employee(Guid id, string fullName, string email, decimal hourlyRate, string passwordHash = "")
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        IsActive = true;
        PasswordHash = passwordHash;
        Update(fullName, email, hourlyRate);
    }

    public void Update(string fullName, string email, decimal hourlyRate)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("Employee name is required.", nameof(fullName));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Employee email is required.", nameof(email));
        }

        if (hourlyRate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(hourlyRate), "Hourly rate cannot be negative.");
        }

        FullName = fullName.Trim();
        Email = email.Trim();
        HourlyRate = hourlyRate;
    }

    public void SetPassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }
}
