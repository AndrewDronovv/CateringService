namespace CateringService.Domain.Repositories;

public interface IUnitOfWorkRepository
{
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}