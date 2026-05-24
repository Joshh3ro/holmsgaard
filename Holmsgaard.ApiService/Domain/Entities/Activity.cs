namespace Holmsgaard.ApiService.Domain.Entities;

public sealed class Activity
{
    public Guid Id { get; }
    public Guid CustomerId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    public Activity(Guid id, Guid customerId, string title, string description)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        IsActive = true;
        Update(customerId, title, description);
    }

    public void Update(Guid customerId, string title, string description)
    {
        if (customerId == Guid.Empty)
        {
            throw new ArgumentException("Customer id is required.", nameof(customerId));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Activity title is required.", nameof(title));
        }

        CustomerId = customerId;
        Title = title.Trim();
        Description = description.Trim();
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
