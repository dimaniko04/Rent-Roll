using RentnRoll.Application.Contracts.Common;

namespace RentnRoll.Application.Contracts.Businesses.GetAllBusinesses;

public record GetAllBusinessesRequest(
    bool IsDeleted = false
) : QueryParams;