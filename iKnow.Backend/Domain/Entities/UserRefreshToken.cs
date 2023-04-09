using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class UserRefreshToken
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public string RefreshToken { get; set; }

    [Required]
    public DateTime ExpiredAt { get; set; }
}
