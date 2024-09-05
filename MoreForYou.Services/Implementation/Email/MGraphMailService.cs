using Microsoft.Extensions.Configuration;
using MoreForYou.Services.Contracts.Email;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Graph.Models;
using Microsoft.Graph;
using Azure.Identity ;
using Google.Apis.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Me.SendMail;

namespace MoreForYou.Services.Implementation.Email
{
    public class MGraphMailService : IMGraphMailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<MGraphMailService> _logger;

        public MGraphMailService(IEmailLogService emailLogService, IConfiguration config,
            ILogger<MGraphMailService> logger)
        {
            //_emailLogService = emailLogService;
            _config = config;
            _logger = logger;
        }


        public async Task SendAsync(string body, List<string> toMails, string subject, long employeeNumber, string benefitName, string[] ccMails = null)
        {
            try
            {
                var scopes = new[] { "https://graph.microsoft.com/.default" };
                var tenantId = _config["GraphMail:TenantId"];
                var clientId = _config["GraphMail:ClientId"];
                var clientSecret = _config["GraphMail:ClientSecret"];
                var clientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecret);

                var graphClient = new GraphServiceClient(clientSecretCredential, scopes);
                List<Recipient> RecipientList = new List<Recipient>();
                List<Recipient> CCRecipientList = new List<Recipient>();
                foreach (string to in toMails)
                {
                    if (!to.Contains("NotProvided"))
                    {
                        new Recipient
                        {
                            EmailAddress = new EmailAddress
                            {
                                Address = to
                            }
                        };
                        RecipientList.Add(
                            new Recipient
                            {
                            EmailAddress = new EmailAddress
                            {
                                Address = to
                            }
                            });
                    }
                }
                if (ccMails != null)
                {
                    foreach (string cc in ccMails)
                    {
                        CCRecipientList.Add(new Recipient
                        {
                            EmailAddress = new EmailAddress()
                            {
                                Address = cc
                            }
                        });
                    }
                    CCRecipientList.Add(
                        new Recipient
                        {
                            EmailAddress = new EmailAddress()
                            {
                                Address = "islammohamed.abdallah@cemex.com"
                            }
                        });
                }
                var requestBody = new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
                {
                    Message = new Message()
                    {
                        Subject = subject,
                        Body = new ItemBody
                        {
                            ContentType = BodyType.Html,
                            Content = body
                        },
                        ToRecipients = RecipientList,
                        CcRecipients = CCRecipientList,
                    }
                };
                bool saveToSentItems = true;
                  await graphClient.Users["doaa.abdel@ext.cemex.com"].SendMail.PostAsync(requestBody);
            }
            catch (Exception ex)
            {
               
            }
        }


        public async Task SendAsync(string body)
        {
            //try
            //{
                var scopes = new[] { "https://graph.microsoft.com/.default" };
                var tenantId = _config["GraphMail:TenantId"];
                var clientId = _config["GraphMail:ClientId"];
                var clientSecret = _config["GraphMail:ClientSecret"];
                var clientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecret);

                var graphClient = new GraphServiceClient(clientSecretCredential, scopes);
                List<Recipient> RecipientList = new List<Recipient>();
                List<Recipient> CCRecipientList = new List<Recipient>();

                new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = "doaa.abdel@ext.cemex.com"
                    }
                };
                RecipientList.Add(
                    new Recipient
                    {
                        EmailAddress = new EmailAddress
                        {
                            Address = "doaa.abdel@ext.cemex.com"
                        }
                    }
                    );
                RecipientList.Add(
                     new Recipient
                     {
                         EmailAddress = new EmailAddress
                         {
                             Address = "islammohamed.abdallah@cemex.com"
                         }
                     }
                    );


                var requestBody = new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
                {
                    Message = new Message()
                    {
                        Subject = "Test Mail",
                        Body = new ItemBody
                        {
                            ContentType = BodyType.Text,
                            Content = "Test Micrsoft Graph API"
                        },
                        ToRecipients = RecipientList,
                        CcRecipients = CCRecipientList,
                    }
                };
                bool saveToSentItems = true;
               var x = graphClient.Users["doaa.abdel@ext.cemex.com"].SendMail.PostAsync(requestBody);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex.ToString());
            //}
        }


        public async Task sendTest()
        {
            var scopes = new[] { "https://graph.microsoft.com/.default" };
            var tenantId = _config["GraphMail:TenantId"];
            var clientId = _config["GraphMail:ClientId"];
            var clientSecret = _config["GraphMail:ClientSecret"];
            var clientSecretCredential = new ClientSecretCredential(
            tenantId, clientId, clientSecret);

            // using Azure.Identity;
            var options = new DeviceCodeCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
                ClientId = clientId,
                TenantId = tenantId,
            };

            // https://learn.microsoft.com/dotnet/api/azure.identity.devicecodecredential
            var deviceCodeCredential = new DeviceCodeCredential(options);

            var graphClient = new GraphServiceClient(deviceCodeCredential, scopes);
            var requestBody = new SendMailPostRequestBody
            {
                Message = new Message
                {
                    Subject = "Meet for lunch?",
                    Body = new ItemBody
                    {
                        ContentType = BodyType.Text,
                        Content = "The new cafeteria is open.",
                    },
                    ToRecipients = new List<Recipient>
        {
            new Recipient
            {
                EmailAddress = new EmailAddress
                {
                    Address = "doaa.abdel@ext.cemex.com",
                },
            },
        },
        //            CcRecipients = new List<Recipient>
        //{
        //    new Recipient
        //    {
        //        EmailAddress = new EmailAddress
        //        {
        //            Address = "danas@contoso.onmicrosoft.com",
        //        },
        //    },
        //},
                },
                SaveToSentItems = false,
            };

            // To initialize your graphClient, see https://learn.microsoft.com/en-us/graph/sdks/create-client?from=snippets&tabs=csharp
            await graphClient.Me.SendMail.PostAsync(requestBody);

        }

    }
}
