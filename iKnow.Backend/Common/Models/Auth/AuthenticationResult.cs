using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Common.Models.Auth;

public class AuthenticationResult
{
    [Required]
    public string AccessToken { get; set; }

    [Required]
    public User User { get; set; }

    public AuthenticationResult(string accessToken, User user)
    {
        AccessToken = accessToken;
        User = user;
    }
}
