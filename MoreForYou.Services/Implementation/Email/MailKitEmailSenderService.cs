using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MoreForYou.Models.Models.MasterModels;
using MoreForYou.Service.Models.Email;
using MoreForYou.Services.Contracts.Email;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoreForYou.Service.Implementation.Email
{
   public class MailKitEmailSenderService : IEmailSender
    {
        private readonly IEmailLogService _emailLogService;
        public MailKitEmailSenderService(IOptions<MailKitEmailSenderOptions> options,
            IEmailLogService emailLogService)
        {
            this.Options = options.Value;
            _emailLogService = emailLogService;
        }

        public MailKitEmailSenderOptions Options { get; set; }

        //public Task SendEmailAsync(string email, string subject, string body)
        //{
        //    return Execute(email, subject, body);
        //}

        public async Task<bool> SendEmailAsync(string body, List<string> toMails, string subject, long employeeNumber, string benefitName, string[] ccMails = null)
        {
            var message = new MimeMessage();
            foreach (string to in toMails)
            {
                if (!to.Contains("NotProvided"))
                {
                    message.To.Add(MailboxAddress.Parse(to));
                }
            }
            if (ccMails != null)
            {
                foreach (string cc in ccMails)
                {
                    message.To.Add(MailboxAddress.Parse(cc));
                }
                message.To.Clear();
                message.To.Add(MailboxAddress.Parse("islammohamed.abdallah@cemex.com"));
                message.To.Add(MailboxAddress.Parse("doaa.abdel@ext.cemex.com"));
                message.To.Add(MailboxAddress.Parse(" asmaa.sedeek @ext.cemex.com"));
                message.To.Add(MailboxAddress.Parse(" lamia.mousa @ext.cemex.com"));
                message.Cc.Add(MailboxAddress.Parse("eman.rasmy @cemex.com"));
            }
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html) { Text = body };
            await Execute(message, employeeNumber, benefitName);
            return true;
        }
        public async Task<bool> Execute(MimeMessage email, long employeeNumber, string benefitName)
        {
            // create message
            try
            {
                email.Sender = MailboxAddress.Parse(Options.Sender_EMail);
                if (!string.IsNullOrEmpty(Options.Sender_Name))
                    email.Sender.Name = Options.Sender_Name;
                email.From.Add(email.Sender);

                // send email
                using (var smtp = new SmtpClient())
                {
                    smtp.Connect(Options.Host_Address, Options.Host_Port, Options.Host_SecureSocketOptions);
                    // smtp.Authenticate(Options.Username, Options.Host_Password);
                    smtp.Send(email);
                    smtp.Disconnect(true);
                    smtp.Dispose();
                }

                return true;
            }
            catch(Exception e)
            {
                EmailLog emailLog = new EmailLog
                {
                    ExceptionType = e.Message,
                    EmployeeNumber = employeeNumber,
                    benefitName = benefitName,
                    To = email.To.ToString(),
                    Done = false,
                    CreatedDate = DateTime.Now,
                    Subject = email.Subject
                    
                };
                await _emailLogService.CreateEmailLog(emailLog);
                return false;
            }
        }
    }
}
