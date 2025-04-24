using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Domain.Entities;
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