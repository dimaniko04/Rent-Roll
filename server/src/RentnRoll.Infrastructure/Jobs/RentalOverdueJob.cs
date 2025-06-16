using Microsoft.Extensions.Logging;

using Quartz;

using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Domain.Entities.Lockers.Enums;
using RentnRoll.Domain.Entities.Rentals;
using RentnRoll.Domain.Entities.Rentals.Enums;

namespace RentnRoll.Infrastructure.Jobs;

public class RentalOverdueJob : IJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RentalOverdueJob> _logger;
    private readonly IRentalRepository _rentalRepository;

    public RentalOverdueJob(
        IUnitOfWork unitOfWork,
        ILogger<RentalOverdueJob> logger)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
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
            ProcessSingleOverdue(rental);
        }
        await _unitOfWork.SaveChangesAsync();

        // Send notifications to users and businesses
    }

    private void ProcessSingleOverdue(Rental overdue)
    {
        _logger.LogInformation(
            "Processing overdue rental: {RentalId}",
            overdue.Id);

        overdue.Status = RentalStatus.Overdue;

        if (overdue.LockerRental is null)
        {
            return;
        }

        var cell = overdue.LockerRental.Cell;

        if (cell is null)
        {
            return;
        }

        cell.Status = CellStatus.Maintenance;
    }
}