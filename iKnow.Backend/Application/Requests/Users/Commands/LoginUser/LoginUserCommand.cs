using Application.Requests.Users.Commands.CreateUser;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Users.Commands.LoginUser;

public class LoginUserCommand : IRequest<User>
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, User>
{
    private readonly IApplicationDbContext _context;

    public LoginUserCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<User> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.Where(u => u.Email == request.Email).FirstOrDefaultAsync();

        if (entity?.Password == request.Password)
            return entity;
        else
            return null;
    }
}
