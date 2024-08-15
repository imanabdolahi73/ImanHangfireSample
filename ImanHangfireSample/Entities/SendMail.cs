namespace SampleHangfire.Entities
{
    public class SendMail
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; } = DateTime.Now;
        public DateTime EndDateTime { get; set; }
        public SendMailStatus SendMailStatus { get; set; }
    }

    public enum SendMailStatus
    {
        Sending = 0,
        Done = 1
    }
}
