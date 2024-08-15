using Hangfire;

namespace SampleHangfire.Infrastrucures
{
    public class SmsService
    {
        private readonly ILogger<SmsService> _logger;
        public SmsService(ILogger<SmsService> logger)
        {
            _logger = logger;
        }

        [Queue("send-sms")]
        [JobDisplayName("ارسال پیام خوش آمد گویی")]
        [AutomaticRetry(Attempts = 3)]
        public void SendWellcome(string PhoneNumber)
        {
            Thread.Sleep(10000);
            _logger.LogInformation($"Welcome message was sent to {PhoneNumber}");
        }

        public void TestJobError()
        {
            throw new Exception("TestJobError");
        }
    }
}
