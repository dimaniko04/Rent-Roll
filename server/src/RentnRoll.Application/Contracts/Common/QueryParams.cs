namespace RentnRoll.Application.Contracts.Common;

public record QueryParams(
    int PageSize = 30,
    int PageNumber = 1,
    string SortBy = "",
    string Filters = ""
);
