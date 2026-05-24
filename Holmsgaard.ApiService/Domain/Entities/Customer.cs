namespace Holmsgaard.ApiService.Domain.Entities;

public sealed class Customer
{
    public Guid Id { get; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; }

    public Customer(Guid id, string name, string email, string phone)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        CreatedAt = DateTimeOffset.UtcNow;
        IsActive = true;
        Update(name, email, phone);
    }

    public void Update(string name, string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Customer name is required.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Customer email is required.", nameof(email));
        }

        Name = name.Trim();
        Email = email.Trim();
        Phone = phone.Trim();
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
