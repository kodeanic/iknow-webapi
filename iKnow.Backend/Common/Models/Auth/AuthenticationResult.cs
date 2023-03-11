using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Common.Models.Auth;

public class AuthenticationResult
{
    [Required]
    public string AccessToken { get; set; }

    [Required]
    public int UserId { get; set; }

    public AuthenticationResult(string accessToken, int userId)
    {
        AccessToken = accessToken;
        UserId = userId;
    }
}
