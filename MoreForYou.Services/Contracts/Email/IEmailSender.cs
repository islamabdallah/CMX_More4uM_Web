using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts.Email
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string body, List<string> toMails, string subject, long employeeNumber, string benefitName, string[] ccMails = null);
    }
}
