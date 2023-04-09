using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class User
{
    public int Id { get; set; }

    [Required]
    [Column("email_phone")]
    public string LoginData { get; set; }
    
    [Required]
    public string PasswordHash { get; set; }
    
    public string? Nickname { get; set; }
}
