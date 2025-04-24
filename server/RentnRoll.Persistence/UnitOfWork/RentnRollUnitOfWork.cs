using Microsoft.Extensions.DependencyInjection;

using RentnRoll.Application.Common.Interfaces.Persistence.Repositories;
using RentnRoll.Application.Common.Interfaces.Persistence.UnitOfWork;
using RentnRoll.Persistence.Context;

namespace RentnRoll.Persistence.UnitOfWork;

public class RentnRollUnitOfWork : IUnitOfWork
{
    private bool _disposed = false;
    private readonly RentnRollDbContext _context;
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, object> _repositories;

    public RentnRollUnitOfWork(
        RentnRollDbContext context,
        IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
        _repositories = new Dictionary<string, object>();
    }

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public TRepository GetRepository<TRepository>()
        where TRepository : IRepository
    {
        var typeName = typeof(TRepository).Name;

        if (!_repositories.ContainsKey(typeName))
        {
            var instance = _serviceProvider.GetService<TRepository>();

            if (instance is null)
            {
                throw new InvalidOperationException(
                    $"Repository of type {typeName} not registered.");
            }

            _repositories.Add(typeName, instance);
        }

        return (TRepository)_repositories[typeName];
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context?.Dispose();
            }

            _disposed = true;
        }
    }

    ~RentnRollUnitOfWork()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}