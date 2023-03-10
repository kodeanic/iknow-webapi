using Application.Requests.Users.Commands.CreateUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest
{
    public int Id { get; }

    public DeleteUserCommand(int id)
    {
        Id = id;
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
        var entity = await _context.Users.Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            return Unit.Value;
            //throw new NotFoundException(nameof(BicycleBrand), request.Id.ToString());
        }
        _context.Users.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
