using Common.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi.Services.Auth;

namespace WebApi.Extensions;

public static class AppAuthenticationExtensions
{
    public static IServiceCollection AddAppAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IJwtTokenService, JwtTokenService>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(jwt =>
        {
            var key = Encoding.ASCII.GetBytes(configuration.GetSection("JwtSettings:SecretKey").Value);

            jwt.SaveToken = true;
            jwt.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false, // !
                ValidateAudience = false, // !
                RequireExpirationTime = false, // !
                ValidateLifetime = true
            };
        });

        return services;
    }
}
