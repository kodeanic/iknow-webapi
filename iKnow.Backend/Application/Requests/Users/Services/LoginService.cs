using Application.Common.Exceptions;
using Common.Models.Auth;
using Common.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Requests.Users.Services;

public class LoginService : ILoginService
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;
    
    public LoginService(IJwtTokenService jwtTokenService, IApplicationDbContext dbContext, IConfiguration configuration)
    {
        _jwtTokenService = jwtTokenService;
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public async Task<AuthenticationResult> Login(User user)
    {
        await DeleteExtraTokens(user.Id);

        return await CreateTokensPair(user);
    }

    private async Task DeleteExtraTokens(int userId)
    {
        var userTokens = await _dbContext.UserRefreshTokens.Where(t => t.UserId == userId).ToListAsync();
        _dbContext.UserRefreshTokens.RemoveRange(userTokens);
    }

    public async Task<AuthenticationResult> Refresh(string oldToken)
    {
        var oldTokenEntity = await GetUserRefreshToken(oldToken);
        
        if(oldTokenEntity.ExpiredAt < DateTime.UtcNow)
            throw new BadRequestException(message: "Старый токен!");

        var user = await _dbContext.Users.Where(u => u.Id == oldTokenEntity.UserId).FirstOrDefaultAsync();

        if (user == null)
        {
            throw new BadRequestException("Нет такого юзера");
        }

        return await CreateTokensPair(user);
    }

    private async Task<AuthenticationResult> CreateTokensPair(User user)
    {
        var accessToken = _jwtTokenService.GenerateJwtToken(user);
        var refreshToken = UpdateRefreshToken(user.Id);

        await _dbContext.SaveChangesAsync();

        return new AuthenticationResult(accessToken, refreshToken, user.Id);
    }

    private string UpdateRefreshToken(int userId)
    {
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        _dbContext.UserRefreshTokens.Add(new UserRefreshToken()
        {
            UserId = userId,
            RefreshToken = refreshToken,
            ExpiredAt = DateTime.UtcNow
                .Add(TimeSpan.Parse(_configuration.GetSection("JwtSettings:RefreshTokenLifetime").Value))
        });

        return refreshToken;
    }
    
    private async Task<UserRefreshToken> GetUserRefreshToken(string oldRefreshToken)
    {
        var oldTokenEntity = await _dbContext.UserRefreshTokens
            .Where(x => x.RefreshToken == oldRefreshToken)
            .FirstOrDefaultAsync();

        if (oldTokenEntity == null)
        {
            throw new Exception(message: "Нет такого токена!");
        }

        _dbContext.UserRefreshTokens.Remove(oldTokenEntity);

        return oldTokenEntity;
    }
}
