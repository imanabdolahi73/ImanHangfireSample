namespace SampleHangfire.Infrastrucures
{
    public class SmsService
    {
        private readonly ILogger<SmsService> _logger;
        public SmsService(ILogger<SmsService> logger)
        {
            _logger = logger;
        }

        public void SendWellcome(string PhoneNumber)
        {
            Thread.Sleep(10000);
            _logger.LogInformation($"Welcome message was sent to {PhoneNumber}");
        }
    }
    public class DatabaseBackupService
    {
        private readonly ILogger<SmsService> _logger;
        public DatabaseBackupService(ILogger<SmsService> logger)
        {
            _logger = logger;
        }

        public void Backup()
        {
            Thread.Sleep(10000);
            _logger.LogInformation($"backup suucessfuly ...");
        }
        public void ArchiveBackup()
        {
            Thread.Sleep(1000);
            _logger.LogInformation($"ArchiveBackup suucessfuly ...");
        }

    }
}
