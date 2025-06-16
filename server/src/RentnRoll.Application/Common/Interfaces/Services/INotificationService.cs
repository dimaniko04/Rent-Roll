using RentnRoll.Domain.Entities.Rentals;

namespace RentnRoll.Application.Common.Interfaces.Services;

public interface INotificationService
{
    Task SendOverdueEmailToUserAsync(
        string userEmail,
        string userFullName,
        Rental rental);
    Task SendOverdueEmailToOwnerAsync(
        string userEmail,
        string userFullName,
        string ownerEmail,
        string ownerFullName,
        Rental rental);
}