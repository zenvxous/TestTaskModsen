using TestTaskModsen.Core.Enums;

namespace TestTaskModsen.Core.Models;

public class User
{
    public User(Guid id, string firstName, string lastName, string email, string passwordHash, UserRole userRole, List<Registration> registrations)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        UserRole = userRole;
        Registrations = registrations;
    }
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserRole UserRole { get; private set; }
    public List<Registration> Registrations { get; private set; }
}