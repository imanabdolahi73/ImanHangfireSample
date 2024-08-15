using SampleHangfire.Data;

namespace SampleHangfire.Infrastrucures
{
    public interface IEmailService
    {
        void SendWellcome(string Email);
        void SendDiscount(string Email);
        void SendNewProducts();
        void SendNews();
        void SendEmail(Guid sendEmailId);
    }

    public class EmailService: IEmailService
    {
        private readonly ILogger<SmsService> _logger;
        private readonly ApplicationDbContext _context;
        public EmailService(ILogger<SmsService> logger , ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
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

        public void SendEmail(Guid sendEmailId)
        {
            var sendMail = _context.SendMails.Find(sendEmailId);
            ArgumentNullException.ThrowIfNull(sendMail);

            //
            for (int i = 1; i < 100; i++)
            {
                Thread.Sleep(100);
            }
            sendMail.SendMailStatus = Entities.SendMailStatus.Done;
            sendMail.EndDateTime = DateTime.Now;
            _context.SaveChanges();
        }
    }
}
