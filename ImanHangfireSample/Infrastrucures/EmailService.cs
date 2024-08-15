using ImanHangfireSample.Filters;
using ImanHangfireSample.Hubs;
using Microsoft.AspNetCore.SignalR;
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
        [LogJob]
        void TestLog();
    }

    public class EmailService: IEmailService
    {
        private readonly ILogger<SmsService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<EmailTrackingHub> _hubContext;
        public EmailService(ILogger<SmsService> logger ,
            ApplicationDbContext context,
            IHubContext<EmailTrackingHub> hubContext)
        {
            _logger = logger;
            _context = context;
            _hubContext = hubContext;
        }
        [LogJob]
        public void TestLog()
        {
            _logger.LogInformation($"------------------------------- Test Log ------------------------------");
            throw new Exception("Error");
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

            double usersCount = 200;
            //
            for (int i = 1; i <= usersCount; i++)
            {
                double percent = ((i / usersCount) * 100);
                Thread.Sleep(500);
                _hubContext.Clients.Group(EmailTrackingHub.GetGroupName(sendEmailId))
                    .SendAsync("ShowStatus", (int)percent,sendEmailId,Faker.User.Email());
            }
            sendMail.SendMailStatus = Entities.SendMailStatus.Done;
            sendMail.EndDateTime = DateTime.Now;
            _context.SaveChanges();

            
        }
    }
}
