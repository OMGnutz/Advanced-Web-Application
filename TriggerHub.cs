using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace _211792H
{
    public class TriggerHub : Hub
    {
        public static void sendMessage()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TriggerHub>();
            context.Clients.All.updateMessages();
        }

        public static void twoFactorAuthenticationsuccess()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TriggerHub>();
            context.Clients.All.addMessage("TFA Authenticated");
        }
    }
}