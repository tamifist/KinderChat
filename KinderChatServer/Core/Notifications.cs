﻿using Microsoft.Azure.NotificationHubs;

namespace KinderChatServer.Core
{
    public class Notifications
    {
        public static Notifications Instance = new Notifications();

        public NotificationHubClient Hub { get; set; }

        private Notifications()
        {
            Hub = NotificationHubClient.CreateClientFromConnectionString(
                "Endpoint=sb://inner6notifications-ns.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=Lu8wegPXUSHfRCqQ2Xfcv6WKF9s7Xai30r1PirMsdLU=",
                "inner6notifications");
        }
    }
}
