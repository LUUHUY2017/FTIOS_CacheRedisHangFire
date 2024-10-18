using EventBus.Messages.Common;
using MassTransit;
using Microsoft.Extensions.Options;

namespace EventBus.Messages;

public class EventBusSettings
{
    public string? HostAddress { get; set; }
    public int? Port { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? QueuePrefix { get; set; }
}
public interface IEventBusAdapter
{
    //Task<bool> CheckRabbitMQStatus();
    Task<ISendEndpoint> GetSendEndpointAsync(string tag);
}


public class EventBusAdapter : IEventBusAdapter
{
    private readonly IBus _bus;
    private readonly EventBusSettings _eventBusSettings;
    public EventBusAdapter(IBus bus, IOptions<EventBusSettings> eventBusSettings)
    {
        _bus = bus;
        _eventBusSettings = eventBusSettings.Value;
    } 

    public async Task<ISendEndpoint> GetSendEndpointAsync(string tag)
    {
        var HostAddress = $"rabbitmq:/{_eventBusSettings.HostAddress}/";
        Uri uri = new Uri($"{HostAddress}{tag}");
        var endPoint = await _bus.GetSendEndpoint(uri);
        return endPoint;
    }

}

