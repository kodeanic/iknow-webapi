using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class User
{
    public int Id { get; set; }

    [Required]
    [Phone]
    public string Phone { get; set; }
    
    [Required]
    public string PasswordHash { get; set; }
    
    public string? Nickname { get; set; }
}
