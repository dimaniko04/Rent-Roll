using RentnRoll.Application.Common.Interfaces.Persistence.Repositories;
using RentnRoll.Core.Entities;
using RentnRoll.Persistence.Context;

namespace RentnRoll.Persistence.Repositories;

public class TestEntityRepository
    : BaseRepository<TestEntity>, ITestEntityRepository
{
    public TestEntityRepository(RentnRollDbContext context)
        : base(context)
    {
    }
}