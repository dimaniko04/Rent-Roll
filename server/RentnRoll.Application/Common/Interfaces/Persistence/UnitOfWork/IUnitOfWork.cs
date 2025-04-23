using RentnRoll.Application.Common.Interfaces.Persistence.Repositories;

namespace RentnRoll.Application.Common.Interfaces.Persistence.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
    TRepository GetRepository<TRepository>()
        where TRepository : IRepository;
}