using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Contracts.Rentals.GetAllRentals;
using RentnRoll.Application.Contracts.Rentals.Response;
using RentnRoll.Domain.Entities.PricingPolicies.Enums;
using RentnRoll.Domain.Entities.Rentals;

namespace RentnRoll.Application.Common.Interfaces.Repositories;

public interface IRentalRepository : IBaseRepository<Rental>
{
    public Task<PaginatedResponse<RentalResponse>>
        GetAllRentalsAsync(
            GetAllRentalsRequest request,
            Guid? businessId = null);
    public Task<ICollection<UserRentalResponse>>
        GetAllUserRentalsAsync(string userId);
    public Task<ICollection<Rental>> GetOverdueRentalsAsync();
}