using Microsoft.AspNetCore.SignalR;

namespace ImanHangfireSample.Hubs
{
    public class EmailTrackingHub:Hub
    {
        public async Task SubscribeSendMail(Guid sendMailId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(sendMailId));
        }

        public static string GetGroupName(Guid sendMailId)
        {
            return $"group-{sendMailId}";
        }
    }
}
