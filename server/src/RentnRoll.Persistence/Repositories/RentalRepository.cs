using Microsoft.EntityFrameworkCore;

using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Contracts.Rentals.GetAllRentals;
using RentnRoll.Application.Contracts.Rentals.Response;
using RentnRoll.Domain.Entities.Rentals;
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
            .Include(r => r.StoreRental!.Store)
            .Include(r => r.StoreRental!.Store!.Assets
                .Where(a => a.BusinessGameId == r.BusinessGameId))
            .ThenInclude(a => a!.BusinessGame)
            .ThenInclude(bg => bg.Game)
            .Include(r => r.LockerRental)
            .Include(r => r.LockerRental!.Locker)
            .Include(r => r.LockerRental!.Locker!.Cells
                .Where(c => c.BusinessGameId == r.BusinessGameId))
            .Join(
                _context.Users,
                r => r.UserId,
                u => u.Id,
                (r, u) => new
                {
                    Rental = r,
                    UserName = u.LastName + " " + u.FirstName
                });

        if (businessId != null)
        {
            joinQuery = joinQuery
                .Where(j => j.Rental.LockerRental!.Locker!.Cells
                    .First().BusinessId == businessId ||
                j.Rental.StoreRental!.Store!.BusinessId == businessId);
        }

        var query = joinQuery
            .Select(j => new RentalResponse(
                j.Rental.Id,
                j.Rental.UserId,
                j.UserName,
                Enum.GetName(j.Rental.Status) ?? "Unknown",
                j.Rental.StartDate,
                j.Rental.EndDate,
                j.Rental.TotalPrice,
                j.Rental.StoreRental != null
                    ? j.Rental.StoreRental.Store!.Address.ToString()
                    : j.Rental.LockerRental!.Locker!.Address.ToString(),
                j.Rental.StoreRental != null
                    ? j.Rental.StoreRental.Store!.Assets!.First().BusinessGame.Game.Name
                    : j.Rental.LockerRental!.Locker!.Cells!.First().BusinessGame!.Game.Name,
                j.Rental.StoreRental != null
                    ? j.Rental.StoreRental.Store!.Name
                    : j.Rental.LockerRental!.Locker!.Name
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
            .Where(r => r.UserId == userId)
            .Include(r => r.StoreRental)
            .Include(r => r.StoreRental!.Store)
            .Include(r => r.StoreRental!.Store!.Assets
                .Where(a => a.BusinessGameId == r.BusinessGameId))
            .ThenInclude(a => a!.BusinessGame)
            .ThenInclude(bg => bg.Game)
            .Include(r => r.LockerRental)
            .Include(r => r.LockerRental!.Locker)
            .Include(r => r.LockerRental!.Locker!.Cells
                .Where(c => c.BusinessGameId == r.BusinessGameId));

        var rentals = await query
            .Select(r => new UserRentalResponse(
                r.Id,
                Enum.GetName(r.Status) ?? "Unknown",
                r.StartDate,
                r.EndDate,
                r.TotalPrice,
                r.StoreRental != null
                    ? r.StoreRental.Store!.Address.ToString()
                    : r.LockerRental!.Locker!.Address.ToString(),
                r.StoreRental != null
                    ? r.StoreRental.Store!.Assets!.First().BusinessGame.Game.Name
                    : r.LockerRental!.Locker!.Cells!.First().BusinessGame!.Game.Name,
                r.StoreRental != null
                    ? r.StoreRental.Store!.Name
                    : r.LockerRental!.Locker!.Name,
                r.LockerRental != null
                    ? r.LockerRental!.Locker!.Cells.First().IotDeviceId
                    : null
            ))
            .ToListAsync();

        return rentals;
    }
}