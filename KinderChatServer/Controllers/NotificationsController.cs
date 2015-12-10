﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using KinderChatServer.Core;

namespace KinderChatServer.Controllers
{
    public class NotificationsController : ApiController
    {
        public async Task<HttpResponseMessage> Post(string toDeviceId, string fromUserId, string fromUserName, string message, string iconUrl)
        {
            var userTag = new string[] { "username:" + toDeviceId };

            var notification = new Dictionary<string, string>
            {
                { "userid", fromUserId },
                { "username", fromUserName },
                { "message", message},
                { "usericon", iconUrl},
                
            };
            await Notifications.Instance.Hub.SendTemplateNotificationAsync(notification, userTag);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
