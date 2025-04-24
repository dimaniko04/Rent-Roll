using RentnRoll.Application.Common.Interfaces.Repositories;

namespace RentnRoll.Application.Common.Interfaces.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
    TRepository GetRepository<TRepository>()
        where TRepository : IRepository;
}