using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace SampleHangfire
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            try
            {
                var httpContext = context.GetHttpContext();
                return httpContext.User.Identity.IsAuthenticated &&
                    httpContext.User.IsInRole("hangfire");
            }
            catch
            {
                return false;
            }
        }
    }
}
