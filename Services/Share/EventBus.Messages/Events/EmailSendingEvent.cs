namespace EventBus.Messages.Events
{
    public class EmailSendingEvent : BaseIntegrationEvent
    {
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string AttachFile { get; set; }
        public string ContentType { get; set; }
    }
    public class AppMobileSendingEvent : BaseIntegrationEvent
    {
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string AttachFile { get; set; }
        public string ContentType { get; set; }
    }
}