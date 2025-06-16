using RentnRoll.Application.Contracts.Businesses.CreateBusiness;
using RentnRoll.Application.Contracts.Businesses.GetAllBusinesses;
using RentnRoll.Application.Contracts.Businesses.Response;
using RentnRoll.Application.Contracts.Businesses.UpdateBusiness;
using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Contracts.Rentals.GetAllRentals;
using RentnRoll.Application.Contracts.Rentals.Response;
using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Services.Businesses;

public interface IBusinessService
{
    Task<PaginatedResponse<BusinessWithOwnerResponse>> GetPaginatedAsync(
        GetAllBusinessesRequest request);
    Task<Result<BusinessWithOwnerResponse>> GetByIdAsync(Guid id);
    Task<Result> BlockAsync(Guid id);
    Task<Result> RestoreAsync(Guid id);

    Task<Result<BusinessResponse>> GetMyBusinessAsync();
    Task<Result<PaginatedResponse<RentalResponse>>>
        GetMyRentalsAsync(GetAllRentalsRequest request);
    Task<Result<BusinessResponse>> CreateAsync(
        CreateBusinessRequest request);
    Task<Result<BusinessResponse>> UpdateAsync(
        UpdateBusinessRequest request);
}