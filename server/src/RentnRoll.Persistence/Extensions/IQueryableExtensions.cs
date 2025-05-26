using Microsoft.EntityFrameworkCore;

using RentnRoll.Application.Common.Interfaces.Contracts;
using RentnRoll.Application.Contracts.Common;

namespace RentnRoll.Persistence.Extensions;

public static class IQueryableExtensions
{
    public static async Task<PaginatedResponse<T>> PaginateAsync<T>(
        this IQueryable<T> query,
        IPaginationRequest paginationRequest)
    {
        var pageNumber = paginationRequest.PageNumber;
        var pageSize = paginationRequest.PageSize;

        if (paginationRequest.PageNumber < 1)
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