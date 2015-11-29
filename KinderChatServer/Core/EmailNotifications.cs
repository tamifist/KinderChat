using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SendGrid;

namespace KinderChatServer.Core
{
    public abstract class EmailNotification
    {
        protected SendGridMessage Message { get; }

        protected EmailNotification()
        {
            Message = new SendGridMessage();
            Message.AddTo("Admin <admin@inner6.com>");
        }

        internal abstract void GenerateMessage();

        public void Send()
        {
            GenerateMessage();

            try
            {
                var credentials = new NetworkCredential("azure_6a6cfd1acd89ad1b240ac3f3f55fb39e@azure.com", "8cbGXNV0jmXK3YN");
                Message.From = new MailAddress("admin@inner6.com", "Inner6");
                var transportWeb = new Web(credentials);
                transportWeb.Deliver(Message);
            }
            catch (Exception ex)
            {
                //Logger.Error("Error sending email", new { Exception = ex });
            }
        }
    }
}
