using RentnRoll.Application.Contracts.Common;
using RentnRoll.Domain.Entities.Rentals.Enums;

namespace RentnRoll.Application.Contracts.Rentals.GetAllRentals;

public record GetAllRentalsRequest(
    RentalStatus? Status,
    DateTime? StartDate,
    DateTime? EndDate
) : QueryParams;