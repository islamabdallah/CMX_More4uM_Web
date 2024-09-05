using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
   public interface IFirebaseNotificationService
    {
        public Message CreateNotification(string title, string notificationBody, string token);
        Task SendNotification(string token, string title, string body);
    }
}
