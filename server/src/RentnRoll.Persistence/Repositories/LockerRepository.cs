using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Domain.Entities.Lockers;
using RentnRoll.Persistence.Context;

namespace RentnRoll.Persistence.Repositories;

public class LockerRepository
    : BaseRepository<Locker>, ILockerRepository
{
    public LockerRepository(RentnRollDbContext context)
        : base(context)
    {
    }
}