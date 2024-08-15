using Hangfire;

namespace SampleHangfire.Infrastrucures
{
    public class ErrorHandlingService
    {
        private readonly ILogger _logger;
        public ErrorHandlingService(ILogger logger)
        {
            _logger = logger;
        }

        public void HandleErrorIfFailed(string jobId)
        {
            using (var connection = JobStorage.Current.GetConnection())
            {
                var parentJobDetails = connection.GetJobData(jobId);
                if (parentJobDetails != null && parentJobDetails.State != "Succeeded")
                {
                    // The job failed, handle the error
                    _logger.LogInformation($"An error jobId {jobId}");
                    // Additional error handling logic here
                }
            }
        }
    }
}
