using Humanizer.Localisation;

using Microsoft.EntityFrameworkCore;

using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Contracts.Rentals.GetAllRentals;
using RentnRoll.Application.Contracts.Rentals.Response;
using RentnRoll.Domain.Entities.Rentals;
using RentnRoll.Domain.Entities.Rentals.Enums;
using RentnRoll.Persistence.Context;
using RentnRoll.Persistence.Extensions;

namespace RentnRoll.Persistence.Repositories;

public class RentalRepository : BaseRepository<Rental>, IRentalRepository
{
    public RentalRepository(RentnRollDbContext context)
        : base(context)
    {
    }

    public Task<PaginatedResponse<RentalResponse>>
        GetAllRentalsAsync(
            GetAllRentalsRequest request,
            Guid? businessId = null)
    {
        var joinQuery = _dbSet
            .AsNoTracking()
            .Include(r => r.StoreRental)
            .Include(r => r.StoreRental!.StoreAsset)
            .ThenInclude(a => a!.BusinessGame)
            .ThenInclude(bg => bg.Game)
            .Include(r => r.StoreRental!.StoreAsset!.Store)
            .Include(r => r.LockerRental)
            .Include(r => r.LockerRental!.Cell)
            .ThenInclude(c => c!.BusinessGame)
            .ThenInclude(bg => bg!.Game)
            .Include(r => r.LockerRental!.Cell!.Locker)
            .Join(
                _context.Users,
                r => r.UserId,
                u => u.Id,
                (r, u) => new
                {
                    Rental = r,
                    u.FullName
                });

        if (businessId != null)
        {
            joinQuery = joinQuery
                .Where(j =>
                    j.Rental
                    .LockerRental!
                    .Cell!.BusinessId == businessId ||
                    j.Rental
                    .StoreRental!
                    .StoreAsset!
                    .Store.BusinessId == businessId);
        }

        var query = joinQuery
            .Select(j => new RentalResponse(
                j.Rental.Id,
                j.Rental.UserId,
                j.FullName,
                Enum.GetName(j.Rental.Status) ?? "Unknown",
                j.Rental.StartDate,
                j.Rental.EndDate,
                j.Rental.TotalPrice,
                j.Rental.StoreRental != null
                    ? j.Rental.StoreRental.StoreAsset!.Store.Address.ToString()
                    : j.Rental.LockerRental!.Cell!.Locker!.Address.ToString(),
                j.Rental.StoreRental != null
                    ? j.Rental.StoreRental.StoreAsset!.BusinessGame.Game.Name
                    : j.Rental.LockerRental!.Cell!.BusinessGame!.Game.Name,
                j.Rental.StoreRental != null
                    ? j.Rental.StoreRental.StoreAsset!.Store.Name
                    : j.Rental.LockerRental!.Cell!.Locker!.Name,
                j.Rental.LockerRental != null
                    ? j.Rental.LockerRental!.Cell!.IotDeviceId
                    : null
            ));

        return query.ToPaginatedResponse(
            request.PageNumber,
            request.PageSize);
    }

    public async Task<ICollection<UserRentalResponse>>
        GetAllUserRentalsAsync(string userId)
    {
        var query = _dbSet
            .AsNoTracking()
            .Include(r => r.StoreRental)
            .Include(r => r.StoreRental!.StoreAsset)
            .ThenInclude(a => a!.BusinessGame)
            .ThenInclude(bg => bg.Game)
            .Include(r => r.StoreRental!.StoreAsset!.Store)
            .Include(r => r.LockerRental)
            .Include(r => r.LockerRental!.Cell)
            .ThenInclude(c => c!.BusinessGame)
            .ThenInclude(bg => bg!.Game)
            .Include(r => r.LockerRental!.Cell!.Locker);

        var rentals = await query
            .Select(r => new UserRentalResponse(
                r.Id,
                Enum.GetName(r.Status) ?? "Unknown",
                r.StartDate,
                r.EndDate,
                r.TotalPrice,
                r.StoreRental != null
                    ? r.StoreRental.StoreAsset!.Store.Address.ToString()
                    : r.LockerRental!.Cell!.Locker!.Address.ToString(),
                r.StoreRental != null
                    ? r.StoreRental.StoreAsset!.BusinessGame.Game.Name
                    : r.LockerRental!.Cell!.BusinessGame!.Game.Name,
                r.StoreRental != null
                    ? r.StoreRental.StoreAsset!.Store.Name
                    : r.LockerRental!.Cell!.Locker!.Name,
                r.LockerRental != null
                    ? r.LockerRental!.Cell!.IotDeviceId
                    : null
            ))
            .ToListAsync();

        return rentals;
    }

    public async Task<ICollection<Rental>> GetOverdueRentalsAsync()
    {
        var query = _dbSet
            .Where(r => r.Status == RentalStatus.Active &&
                        r.EndDate < DateTime.UtcNow)
            .Include(r => r.LockerRental)
            .ThenInclude(lr => lr!.Cell)
            .ThenInclude(c => c!.BusinessGame)
            .ThenInclude(bg => bg!.Game)
            .Include(r => r.LockerRental!.Cell)
            .ThenInclude(c => c!.Business)
            .Include(r => r.StoreRental)
            .ThenInclude(sr => sr!.StoreAsset)
            .ThenInclude(sa => sa!.BusinessGame)
            .ThenInclude(bg => bg!.Game)
            .Include(r => r.StoreRental!.StoreAsset)
            .ThenInclude(sa => sa!.Store)
            .ThenInclude(s => s!.Business);

        var overdue = await query.ToListAsync();

        return overdue;
    }
}