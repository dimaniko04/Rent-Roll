using Microsoft.EntityFrameworkCore;

using RentnRoll.Application.Contracts.Common;

namespace RentnRoll.Persistence.Extensions;

public static class IQueryableExtensions
{
    public static async Task<PaginatedResponse<T>> ToPaginatedResponse<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than 0.");
        }

        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than 0.");
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResponse<T>(
            items,
            totalCount,
            pageNumber,
            pageSize
        );
    }
}