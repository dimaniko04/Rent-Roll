namespace RentnRoll.Application.Contracts.Common;

public class PaginatedResponse<T>
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public List<T> Items { get; set; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public PaginatedResponse(
        IEnumerable<T> items,
        int count,
        int pageNumber,
        int pageSize)
    {
        CurrentPage = pageNumber;
        PageSize = pageSize;
        TotalCount = count;
        Items = items.ToList();
    }

    public static PaginatedResponse<T> Map<TIn>(
        PaginatedResponse<TIn> source,
        Func<TIn, T> mapFunc)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (mapFunc == null) throw new ArgumentNullException(nameof(mapFunc));

        return new PaginatedResponse<T>(
            source.Items.Select(mapFunc),
            source.TotalCount,
            source.CurrentPage,
            source.PageSize);
    }
}