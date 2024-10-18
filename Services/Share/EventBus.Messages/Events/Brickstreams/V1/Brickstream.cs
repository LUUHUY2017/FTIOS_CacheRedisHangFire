namespace EventBus.Messages.Events.Brickstreams.V1;

public class Brickstream
{
    public DateTime ReceivedTime { get; set; } = DateTime.Now;
    public string Content { get; set; }
    public string ReceivedIp { get; set; }
}