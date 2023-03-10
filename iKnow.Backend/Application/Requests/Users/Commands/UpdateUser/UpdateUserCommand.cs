using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using SwaggerIgnore = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace Application.Requests.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest
{
    [SwaggerIgnore]
    public int Id { get; set; }

    [Required]
    public string Login { get; set; }

    [Required]
    public string Password { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (entity == null)
        {
            return Unit.Value;
            //throw new NotFoundException(nameof(BicycleBrand), request.Id.ToString());
        }

        entity.Login = request.Login;
        entity.Password = request.Password;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
