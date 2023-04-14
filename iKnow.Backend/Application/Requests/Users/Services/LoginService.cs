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
        var userTokens = await _dbContext.UserRefreshTokens
            .Include(t => t.User)
            .Where(t => t.User.Id == userId).ToListAsync();
        
        _dbContext.UserRefreshTokens.RemoveRange(userTokens);
    }

    public async Task<AuthenticationResult> Refresh(string oldToken)
    {
        var oldTokenEntity = await GetUserRefreshToken(oldToken);
        
        if(oldTokenEntity.ExpiredAt < DateTime.UtcNow)
            throw new BadRequestException("Токен больше не валиден");

        var user = await _dbContext.Users.Where(u => u.Id == oldTokenEntity.User.Id).FirstOrDefaultAsync();

        if (user == null)
        {
            throw new BadRequestException("Пользователя не существует");
        }

        return await CreateTokensPair(user);
    }

    private async Task<AuthenticationResult> CreateTokensPair(User user)
    {
        var accessToken = _jwtTokenService.GenerateJwtToken(user);
        var refreshToken = UpdateRefreshToken(user);

        await _dbContext.SaveChangesAsync();

        return new AuthenticationResult(accessToken, refreshToken, user.Id);
    }

    private string UpdateRefreshToken(User user)
    {
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        _dbContext.UserRefreshTokens.Add(new UserRefreshToken()
        {
            User = user,
            RefreshToken = refreshToken,
            ExpiredAt = DateTime.UtcNow
                .Add(TimeSpan.Parse(_configuration.GetSection("JwtSettings:RefreshTokenLifetime").Value))
        });

        return refreshToken;
    }
    
    private async Task<UserRefreshToken> GetUserRefreshToken(string oldRefreshToken)
    {
        var oldTokenEntity = await _dbContext.UserRefreshTokens
            .Include(t => t.User)
            .Where(t => t.RefreshToken == oldRefreshToken)
            .FirstOrDefaultAsync();

        if (oldTokenEntity == null)
        {
            throw new BadRequestException("Токен больше не валиден");
        }

        _dbContext.UserRefreshTokens.Remove(oldTokenEntity);

        return oldTokenEntity;
    }
}
