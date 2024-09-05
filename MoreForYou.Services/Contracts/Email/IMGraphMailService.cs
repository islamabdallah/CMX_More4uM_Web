using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts.Email
{
    public interface IMGraphMailService
    {
         Task SendAsync(string body, List<string> toMails, string subject, long employeeNumber, string benefitName, string[] ccMails = null);

        public  Task SendAsync(string body);

        public Task sendTest();


    }
}
