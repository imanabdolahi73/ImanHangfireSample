namespace SampleHangfire.Infrastrucures
{
    public class EmailService
    {
        private readonly ILogger<SmsService> _logger;
        public EmailService(ILogger<SmsService> logger)
        {
            _logger = logger;
        }

        public void SendWellcome(string Email)
        {
            Thread.Sleep(10000);
            _logger.LogInformation($"Welcome email was sent to {Email}");
        }
        public void SendDiscount(string Email)
        {
            Thread.Sleep(10000);
            _logger.LogInformation($"Discount email was sent to {Email}");
        }
        public void SendNewProducts()
        {
            Thread.Sleep(2000);
            _logger.LogInformation($"New Products was sent ...");
        }
        public void SendNews()
        {
            Thread.Sleep(2000);
            _logger.LogInformation($"News was sent ...");
        }
    }
}
