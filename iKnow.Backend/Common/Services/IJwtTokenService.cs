using Domain.Entities;

namespace Common.Services;

public interface IJwtTokenService
{
    string GenerateJwtToken(User user);

    public string GenerateRefreshToken();
}
