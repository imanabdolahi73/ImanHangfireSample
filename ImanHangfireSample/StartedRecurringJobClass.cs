using Hangfire;
using SampleHangfire.Infrastrucures;

namespace SampleHangfire
{
    public class StartedRecurringJobClass : IHostedService
    {
        [Obsolete]
        public Task StartAsync(CancellationToken cancellationToken)
        {
            RecurringJob.AddOrUpdate<IEmailService>("RecurringJob_With_IHostedService", p => p.SendNewProducts(),Cron.Minutely());
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
