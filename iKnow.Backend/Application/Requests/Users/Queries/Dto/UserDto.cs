using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Users.Queries.Dto;

public class UserDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Login { get; set; }

    [Required]
    public string Password { get; set; }
}
