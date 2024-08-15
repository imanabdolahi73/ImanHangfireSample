using Hangfire;
using Microsoft.AspNetCore.Mvc;
using SampleHangfire.Data;
using SampleHangfire.Entities;
using SampleHangfire.Infrastrucures;

namespace ImanHangfireSample.Controllers
{
    public class EmailSenderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBackgroundJobClient _jobClient;
        private readonly IEmailService _emailService;
        public EmailSenderController(ApplicationDbContext context,
            IBackgroundJobClient jobClient,
            IEmailService emailService)
        {
            _context = context;
            _jobClient = jobClient;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            var emails = _context.SendMails.OrderByDescending(p => p.StartDateTime).ToList();
            return View(emails);
        }

        public IActionResult Detail(Guid Id)
        {
            var email = _context.SendMails.Find(Id);
            ArgumentNullException.ThrowIfNull(email);
            return View(email);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(MailDto mail)
        {
            ArgumentNullException.ThrowIfNull(mail);
            SendMail sendMail = new SendMail()
            {
                Subject = mail.Subject,
                Body = mail.Body
            };
            _context.SendMails.Add(sendMail);
            _context.SaveChanges();

            _jobClient.Enqueue<IEmailService>(p => p.SendEmail(sendMail.Id));

            return RedirectToAction(nameof(Index));
        }
    }

    public class MailDto
    {
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
