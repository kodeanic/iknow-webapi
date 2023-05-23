namespace Domain.Entities;

public class User
{
    public int Id { get; set; }

    public int PictureId { get; set; }
    
    public string Phone { get; set; }
    
    public string PasswordHash { get; set; }
    
    public string? Nickname { get; set; }
}
