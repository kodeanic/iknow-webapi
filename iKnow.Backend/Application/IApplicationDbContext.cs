using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application;

public interface IApplicationDbContext : IDisposable
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
    
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Subtopic> Subtopics { get; set; }
    
    public DbSet<Progress> Progresses { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    int SaveChanges();
}
