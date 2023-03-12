using Common.Models.Auth;
using Domain.Entities;

namespace Application.Requests.Users.Services;

public interface ILoginService
{
    Task<AuthenticationResult> Login(User user);

    Task<AuthenticationResult> Refresh(string oldToken);
}
