namespace Domain.Entities;

public class UserRefreshToken
{
    public int Id { get; set; }

    public User User { get; set; }
    
    public string RefreshToken { get; set; }

    public DateTime ExpiredAt { get; set; }
}
