using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using MoreForYou.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Implementation
{
    public class FirebaseNotificationService : IFirebaseNotificationService
    {
        private readonly FirebaseMessaging messaging;

        public FirebaseNotificationService()
        {
            var app = FirebaseApp.DefaultInstance;
            if (app == null)
            {
                app = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("cemex-prd-gbl-firebase-adminsdk-vtcd1-d2c74f09c9.json").
                    CreateScoped("https://www.googleapis.com/auth/firebase.messaging")
                });
            }
            messaging = FirebaseMessaging.GetMessaging(app);

        }
        public Message CreateNotification(string title, string notificationBody, string token)
        {
            var message = new Message()
            {
                Token = token,
                //Topic = "news",
                Notification = new Notification()
                {
                    Body = notificationBody,
                    Title = title,
                }
            };



            return message;
        }

        public async Task SendNotification(string token, string title, string body)
        {
            //System.Net.ServicePointManager.Expect100Continue = false;
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
              //  await messaging.SendAsync(CreateNotification(title, body, token));
        }
    }
}