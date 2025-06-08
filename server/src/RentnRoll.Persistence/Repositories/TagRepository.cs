using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Domain.Entities.Tags;
using RentnRoll.Persistence.Context;

namespace RentnRoll.Persistence.Repositories;

public class TagRepository
    : BaseRepository<Tag>, ITagRepository
{
    public TagRepository(RentnRollDbContext context)
        : base(context)
    {
    }
}