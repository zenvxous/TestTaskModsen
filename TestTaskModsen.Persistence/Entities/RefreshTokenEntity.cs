
namespace TestTaskModsen.Persistence.Entities;

public class RefreshTokenEntity
{
    public Guid UserId { get; set; }
    
    public UserEntity User { get; set; }
    
    public string Token { get; set; } = string.Empty;
    
    public DateTime Expiration { get; set; }
}