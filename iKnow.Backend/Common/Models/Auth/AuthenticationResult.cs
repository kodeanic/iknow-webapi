using System.ComponentModel.DataAnnotations;

namespace Common.Models.Auth;

public class AuthenticationResult
{
    [Required]
    public string AccessToken { get; set; }

    [Required]
    public string RefreshToken { get; set; }

    [Required]
    public int UserId { get; set; }

    public AuthenticationResult(string accessToken, string refreshToken, int userId)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        UserId = userId;
    }
}
