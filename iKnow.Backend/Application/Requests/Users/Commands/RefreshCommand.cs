using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Users.Commands;

public class RefreshCommand
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
