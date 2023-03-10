using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application;

public interface IApplicationDbContext : IDisposable
{
    public DbSet<User> Users { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    int SaveChanges();
}
