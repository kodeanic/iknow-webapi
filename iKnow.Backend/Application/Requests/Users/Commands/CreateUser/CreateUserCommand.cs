using Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<int>
{
    [Required]
    public string Login { get; set; }

    [Required]
    public string Password { get; set; }
}

public class CreateUserCommandHandler: IRequestHandler<CreateUserCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateUserCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = new User()
        {
            Login = request.Login,
            Password = request.Password
        };

        await _context.Users.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
        /*
        var entity = _mapper.Map<Bicycle>(request);
        _context.Users.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
        */
    }
}
