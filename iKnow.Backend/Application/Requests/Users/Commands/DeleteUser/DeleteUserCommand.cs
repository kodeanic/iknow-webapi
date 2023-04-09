using Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest
{
    public string LoginData { get; }

    public DeleteUserCommand(string loginData)
    {
        LoginData = loginData;
    }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users
            .Where(x => x.LoginData == request.LoginData)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (entity == null)
        {
            throw new NotFoundException("Такого пользователя не существует");
        }
        _context.Users.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
