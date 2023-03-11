using Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<User>
{
    [Required]
    public string Login { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
{
    private readonly IApplicationDbContext _context;

    public CreateUserCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = new User()
        {
            Login = request.Login,
            Email = request.Email,
            Password = request.Password
        };

        await _context.Users.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}
