namespace RentnRoll.Application.Common.Interfaces.Services;

public interface IMqttPublisher
{
    Task PublishLockerConfigAsync(
        string deviceId,
        ICollection<(Guid cellId, int pin)> config);
}