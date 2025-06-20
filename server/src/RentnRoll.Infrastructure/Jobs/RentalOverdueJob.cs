using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using Quartz;

using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.Services;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Domain.Entities.Rentals;
using RentnRoll.Domain.Entities.Rentals.Enums;
using RentnRoll.Persistence.Identity;

namespace RentnRoll.Infrastructure.Jobs;

public class RentalOverdueJob : IJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RentalOverdueJob> _logger;
    private readonly IRentalRepository _rentalRepository;
    private readonly INotificationService _notificationService;

    public RentalOverdueJob(
        IUnitOfWork unitOfWork,
        UserManager<User> userManager,
        ILogger<RentalOverdueJob> logger,
        INotificationService notificationService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _notificationService = notificationService;
        _rentalRepository = _unitOfWork
            .GetRepository<IRentalRepository>();
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Executing RentalOverdueJob at {Time}", DateTimeOffset.Now);

        try
        {
            await ProcessOverdueRentalsAsync();
            _logger.LogInformation(
                "RentalOverdueJob completed successfully at {Time}",
                DateTimeOffset.Now);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "RentalOverdueJob failed at {Time}",
                DateTimeOffset.Now);
        }
    }

    private async Task ProcessOverdueRentalsAsync()
    {
        ICollection<Rental> overdueRentals;

        try
        {
            overdueRentals = await _rentalRepository
                .GetOverdueRentalsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve overdue rentals");
            throw new InvalidOperationException(
                "Failed to retrieve overdue rentals", ex);
        }

        if (overdueRentals.Count == 0)
        {
            _logger.LogInformation("No overdue rentals found.");
            return;
        }

        foreach (var rental in overdueRentals)
        {
            await ProcessSingleOverdue(rental);
        }
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task ProcessSingleOverdue(Rental overdue)
    {
        _logger.LogInformation(
            "Processing overdue rental: {RentalId}",
            overdue.Id);

        overdue.Status = RentalStatus.Overdue;

        var user = await _userManager
            .FindByIdAsync(overdue.UserId);
        var owner = await _userManager
            .FindByIdAsync(overdue.Business.OwnerId);

        if (user == null || owner == null)
        {
            _logger.LogWarning(
                "User or owner not found for rental {RentalId}.",
                overdue.Id);
            throw new InvalidOperationException(
                $"Rental {overdue.Id} has no associated User or Owner.");
        }

        await SendNotificationsAsync(
            user,
            owner,
            overdue);
    }

    private async Task SendNotificationsAsync(
        User user,
        User owner,
        Rental overdue)
    {
        await _notificationService
            .SendOverdueEmailToUserAsync(
                user.Email!,
                user.FullName!,
                overdue);
        await _notificationService
            .SendOverdueEmailToOwnerAsync(
                user.Email!,
                user.FullName!,
                owner.Email!,
                owner.FullName!,
                overdue);
    }
}