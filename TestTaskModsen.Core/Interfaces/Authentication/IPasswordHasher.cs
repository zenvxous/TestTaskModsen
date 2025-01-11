namespace TestTaskModsen.Core.Interfaces.Authentication;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPasswordHash(string password, string passwordHash);
}