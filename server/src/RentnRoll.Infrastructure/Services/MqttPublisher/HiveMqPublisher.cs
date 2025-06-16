using System.Text.Json;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MQTTnet;
using MQTTnet.Protocol;

using RentnRoll.Application.Common.Interfaces.Services;

namespace RentnRoll.Infrastructure.Services.MqttPublisher;

public class HiveMqPublisher : IMqttPublisher
{
    private readonly HiveMqSettings _settings;
    private readonly ILogger<HiveMqPublisher> _logger;

    public HiveMqPublisher(
        IOptions<HiveMqSettings> options,
        ILogger<HiveMqPublisher> logger)
    {
        _logger = logger;
        _settings = options.Value;
    }

    public async Task PublishLockerConfigAsync(
        string deviceId,
        ICollection<(Guid cellId, int pin)> config)
    {
        var factory = new MqttClientFactory();
        using var client = factory.CreateMqttClient();

        try
        {
            if (!client.IsConnected)
                await ConnectAsync(client);

            _logger.LogInformation("Connected to HiveMQ broker at {Host}:{Port}",
                _settings.Host, _settings.Port);

            var topic = $"locker/{deviceId}/configure";
            var payload = JsonSerializer.Serialize(
                new
                {
                    cells = config.Select(c => new
                    {
                        cellId = c.cellId.ToString(),
                        c.pin
                    }) ?? []
                });

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel(
                    MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            await client.PublishAsync(message);
            _logger.LogInformation("Published message to topic {Topic}", topic);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish message to HiveMQ broker");
            throw new InvalidOperationException(
                "Failed to publish message to HiveMQ broker", ex);
        }
        finally
        {
            await DisconnectAsync(client);
            _logger.LogInformation("Disconnected from HiveMQ broker");
        }
    }

    private async Task ConnectAsync(IMqttClient client)
    {
        var options = new MqttClientOptionsBuilder()
            .WithTimeout(TimeSpan.FromSeconds(60))
            .WithTcpServer(_settings.Host, _settings.Port)
            .WithCredentials(_settings.Username, _settings.Password)
            .WithTlsOptions(new MqttClientTlsOptions
            {
                UseTls = true,
                AllowUntrustedCertificates = true,
                IgnoreCertificateChainErrors = true,
                IgnoreCertificateRevocationErrors = true,
                CertificateValidationHandler = context => true
            })
            .Build();

        _logger.LogInformation("Connecting to HiveMQ broker at {Host}:{Port}",
            _settings.Host, _settings.Port);

        await client.ConnectAsync(options);
    }

    private async Task DisconnectAsync(IMqttClient client)
    {
        if (client.IsConnected)
        {
            var disconnectOptions = new MqttClientDisconnectOptionsBuilder()
                .WithReason(MqttClientDisconnectOptionsReason
                    .NormalDisconnection)
                .Build();

            await client.DisconnectAsync(disconnectOptions);
            _logger.LogInformation("Disconnected from HiveMQ broker");
        }
    }
}