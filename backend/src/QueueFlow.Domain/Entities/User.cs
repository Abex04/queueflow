namespace QueueFlow.Domain.Entities;

public class User
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = default!;
    public string Role { get; private set; } = default!; // "Citizen" | "Staff"

    private User() { } // EF Core

    public User(string name, string role)
    {
        Name = name;
        Role = role;
    }
}
