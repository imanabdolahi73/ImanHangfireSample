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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Continuation()
        {
            var jobId = BackgroundJob.Enqueue<DatabaseBackupService>(p => p.Backup());
            BackgroundJob.ContinueJobWith<DatabaseBackupService>(jobId, p => p.ArchiveBackup());

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