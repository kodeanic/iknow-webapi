using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;
using Application.Requests.Users.Services;
using Common.Services;
using Microsoft.Extensions.Configuration;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddScoped<ILoginService>(s => new LoginService(
            s.GetRequiredService<IJwtTokenService>(),
            s.GetRequiredService<IApplicationDbContext>(),
            s.GetRequiredService<IConfiguration>()));

        return services;
    }
}
