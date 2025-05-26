namespace RentnRoll.Application.Common.Interfaces.Contracts;

public interface IPaginationRequest
{
    int PageNumber { get; init; }
    int PageSize { get; init; }
}