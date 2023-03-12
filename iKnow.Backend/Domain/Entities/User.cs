using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class User
{
    public int Id { get; set; }

    [Required]
    public string Login { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    public string? SomeDescription { get; set; }
}
