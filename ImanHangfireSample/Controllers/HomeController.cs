using Hangfire;
using Microsoft.AspNetCore.Mvc;
using SampleHangfire.Infrastrucures;
using SampleHangfire.Models;
using System.Diagnostics;

namespace SampleHangfire.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBackgroundJobClient _backgroundJobClient;
        public HomeController(ILogger<HomeController> logger, IBackgroundJobClient backgroundJobClient)
        {
            _logger = logger;
            _backgroundJobClient = backgroundJobClient;
        }

        public IActionResult Index()
        {
            BackgroundJob.Enqueue<SmsService>(p=>p.SendWellcome("09120000000"));
            _backgroundJobClient.Enqueue<IEmailService>(p => p.SendWellcome("iman@gmail.com"));
            //var jobId = BackgroundJob.Enqueue<SmsService>(p => p.TestJobError());
            return View();
        }

        public IActionResult Continuation()
        {
            var jobId = _backgroundJobClient.Enqueue<DatabaseBackupService>(p => p.Backup());
            _backgroundJobClient.ContinueJobWith<DatabaseBackupService>(jobId, p => p.ArchiveBackup() , JobContinuationOptions.OnlyOnSucceededState);
            _backgroundJobClient.ContinueJobWith<ErrorHandlingService>(jobId, p => p.HandleErrorIfFailed(jobId));

            return RedirectToAction(nameof(HomeController.Index), "home");
        }

        [Obsolete]
        public IActionResult RunJob()
        {
            RecurringJob.Trigger("RecurringJob_With_IHostedService");
            return RedirectToAction(nameof(HomeController.Index), "home");
        }

        public IActionResult DeleteJob()
        {
            RecurringJob.RemoveIfExists("RecurringJob_With_LifeTime");
            return RedirectToAction(nameof(HomeController.Index), "home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}