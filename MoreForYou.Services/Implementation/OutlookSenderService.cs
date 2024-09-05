using MoreForYou.Services.Contracts;
using MoreForYou.Services.Contracts.Email;
using MoreForYou.Services.Models;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Implementation
{
    public class OutlookSenderService : IOutlookSenderService
    {
        private readonly IEmployeeService _employeeService;
        private readonly IEmailSender _emailSender;


        public OutlookSenderService(IEmployeeService employeeService, IEmailSender emailSender)
        {
            _employeeService = employeeService;
            _emailSender = emailSender;
        }
        public async Task<bool> SendFromOutlook(string body, List<string> toMails, string subject, string[] ccMails = null)
        {
            MailAddress from = new MailAddress("test@cemex.com");
            MailMessage message = new MailMessage();
            foreach (string to in toMails)
            {
                if (!to.Contains("NotProvided"))
                {
                    message.To.Add(to);
                }
            }
            if (ccMails != null)
            {
                foreach (string cc in ccMails)
                {
                    message.To.Add(cc);
                }
                message.To.Add("doaa.abdel@ext.cemex.com");
            }
            message.From = from;
            message.Subject = subject;//"Automatic Mail";
            message.Body = body;
            message.IsBodyHtml = true;

            //message.c
            //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.office365.com", 587)

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("rsmtp.cemex.com", 25)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("doaa.salhen@outlook.com", "iop123@@"),
                EnableSsl = true,
                //Timeout = 120000,
                // specify whether your host accepts SSL connections
            };
            // code in brackets above needed if authentication required
            try
            {
                // client.Send(message);
                return true;
            }
            catch (SmtpException ex)
            {
                return false;
            }
        }


        public string PrapareMailBody(BenefitRequestModel model)
        {
            string body = string.Empty;
            //string filePath = "D:/Cemex Project/gitHubProject/Cemex Backulaing/DevArea/Core/CoreServices/MailService/EmailTemplate.html";

            string fileName = "EmailBody.html";
            if (model.SendTo != 0)
            {
                fileName = "ThankYouMail.html";
            }
                string path = Path.Combine(@"D:\_cemex\_projects\_AzureMore4U\More4UAzure\MoreForYou.Services\MailTemplate\", fileName);
            string imagePath = CommanData.Url + CommanData.CardsFolder + model.Benefit.BenefitCard;

            using (StreamReader reader = new StreamReader(path))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{benefitName}", model.Benefit.Name);
            body = body.Replace("{imgSrc}", model.Benefit.BenefitCard);
            body = body.Replace("{employeeNumber}", "  " + model.Employee.EmployeeNumber.ToString());
            body = body.Replace("{employeeName}", "  " + model.Employee.FullName.ToString());
            body = body.Replace("{DateFrom}", "  " + model.ExpectedDateFrom.ToString("yyyy-MM-dd"));
            body = body.Replace("{DateTo}", "  " + model.ExpectedDateTo.ToString("yyyy-MM-dd"));
            if (model.SendTo != 0)
            {
                EmployeeModel thankedEmployee = _employeeService.GetEmployee(model.SendTo).Result;
                body = body.Replace("{employeeToNumber}", "  " + thankedEmployee.EmployeeNumber.ToString());
                body = body.Replace("{employeeToName}", "  " + thankedEmployee.FullName.ToString());
            }
            else
            {
                body = body.Replace("{GiftData}", "");
            }
            return body;
        }

        public string PrapareGroupMailBody(string BenefitImage, string benefitName, DateTime from, DateTime to, List<GroupEmployeeModel> groupEmployeeModels)
        {
            string body = string.Empty;


            string fileName = "EmailGroupBody.html";
            string Names = "";
            string Numbers = "";
            //string path = Path.Combine(@"D:\Work\MoreForYou\Project_26_12_2022 (2)\Project\MoreForYou.Services", @"MailTemplate\", fileName);
            string path = Path.Combine(@"D:\_cemex\_projects\_AzureMore4U\More4UAzure\MoreForYou.Services\MailTemplate\", fileName);

            string imagePath = CommanData.Url + CommanData.CardsFolder + BenefitImage;

            using (StreamReader reader = new StreamReader(path))
            {
                body = reader.ReadToEnd();
            }
            foreach (var employee in groupEmployeeModels)
            {
                Names = Names + "<tr><td>" + employee.Employee.FullName + "</td>" +
                    "<td>" + employee.Employee.EmployeeNumber + "</td> </tr>";
            }
            body = body.Replace("{benefitName}", benefitName);
            body = body.Replace("{imgSrc}", imagePath);
            body = body.Replace("{employeeData}", Names.ToString());
            body = body.Replace("{DateFrom}", "   " + from.ToString("yyyy-MM-dd"));
            body = body.Replace("{DateTo}", "   " + to.ToString("yyyy-MM-dd"));

            return body;
        }
        public string PrapareSupervisorMailBodyForIndividualBenefit(string BenefitImage, string benefitName, long EmployeeNumber, string employeeName)
        {
            string body = string.Empty;


            string fileName = "SupervisorEmailBodyForIndividualBenefit.html";
            string path = Path.Combine(@"D:\_cemex\_projects\_AzureMore4U\More4UAzure\MoreForYou.Services\MailTemplate", fileName);
            //tring path = Path.Combine(@"D:\Work\MoreForYou\Project_26_12_2022 (2)\Project\MoreForYou.Services", @"MailTemplate\", fileName);
            string imagePath = CommanData.Url + CommanData.CardsFolder + BenefitImage;
            using (StreamReader reader = new StreamReader(path))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{benefitName}", benefitName);
            //body = body.Replace("{imgSrc}", imagePath);
            body = body.Replace("{employeeNumber}", EmployeeNumber.ToString());
            body = body.Replace("{employeeName}", employeeName);
            return body;
        }

        public string PrapareSupervisorMailBodyForGroupBenefit(string BenefitImage, string benefitName, long EmployeeNumber, string employeeName)
        {
            string body = string.Empty;
            //string filePath = "D:/Cemex Project/gitHubProject/Cemex Backulaing/DevArea/Core/CoreServices/MailService/EmailTemplate.html";

            string fileName = "SupervisorEmailBodyForGroupBenefit.html";
            // string path = Path.Combine(@"C:\_cemex\_projects\_more4u\MoreForeYou\MoreForYou.Services", @"MailTemplate\", fileName);
            string path = Path.Combine(@"D:\_cemex\_projects\_AzureMore4U\More4UAzure\MoreForYou.Services\MailTemplate", fileName);
            string imagePath = CommanData.Url + CommanData.CardsFolder + BenefitImage;
            using (StreamReader reader = new StreamReader(path))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{benefitName}", benefitName);
            body = body.Replace("{imgSrc}", imagePath);
            return body;
        }

        public string PrapareUserDataMailBody(long EmployeeNumber, string employeeName, string password)
        {
            string body = string.Empty;
            //string filePath = "D:/Cemex Project/gitHubProject/Cemex Backulaing/DevArea/Core/CoreServices/MailService/EmailTemplate.html";

            string fileName = "EmailBodySendUserData.html";
            //string path = Path.Combine(@"C:\_cemex\_projects\_more4u\MoreForeYou\MoreForYou.Services\", @"MailTemplate\", fileName);
            string path = Path.Combine(@"D:\_cemex\_projects\_AzureMore4U\More4UAzure\MoreForYou.Services\MailTemplate\", fileName);
            string imagePath = "http://20.86.97.165/more4u/images/logo.PNG";
            using (StreamReader reader = new StreamReader(path))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{employeeName}", employeeName);
            body = body.Replace("{employeeNumber}", EmployeeNumber.ToString());
            body = body.Replace("{password}", password);
            body = body.Replace("{imgSrc}", imagePath);
            return body;
        }

        public async Task<bool> SendMailForApproval(string To, BenefitModel benefitModel, EmployeeModel requester, long benefitType, string CC = null)
        {
            try
            {
                string mailBody = string.Empty;
                bool mailResult = false;
                if (benefitType == (long)CommanData.BenefitTypes.Individual)
                {
                    mailBody = PrapareSupervisorMailBodyForIndividualBenefit(benefitModel.BenefitCard, benefitModel.Name, requester.EmployeeNumber, requester.FullName);
                }
                else if (benefitType == (long)CommanData.BenefitTypes.Group)
                {
                    mailBody = PrapareSupervisorMailBodyForGroupBenefit(benefitModel.BenefitCard, benefitModel.Name, requester.EmployeeNumber, requester.FullName);
                }

                List<string> toMails = new List<string>()
                {
                    To
                };
                if (CC == null)
                {
                    mailResult = SendFromOutlook(mailBody, toMails, "More4U approval", null).Result;
                    mailResult = _emailSender.SendEmailAsync(mailBody, toMails, "More4u [" + benefitModel.Name + "] _ Approval", requester.EmployeeNumber,benefitModel.Name, null).IsCompletedSuccessfully;
                }
                else
                {
                    string[] ccMails = new string[] { CC };
                  mailResult = SendFromOutlook(mailBody, toMails,"More4U approval", ccMails).Result;
                  mailResult = await _emailSender.SendEmailAsync(mailBody, toMails, "More4u "+ benefitModel.Name+ " Approval", requester.EmployeeNumber, benefitModel.Name, ccMails);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public bool SendMailWithUserData(string To, string EmployeeName, long EmployeeNumber, string password)
        {
            try
            {
                string mailBody = string.Empty;
                bool mailResult = false;
                string CC = null;
                mailBody = PrapareUserDataMailBody(EmployeeNumber, EmployeeName, password);
                List<string> toMails = new List<string>()
                {
                    To
                };

                if (CC == null)
                {
                    //mailResult = SendFromOutlook(mailBody, toMails, null).Result;
                }
                else
                {
                    string[] ccMails = new string[] { CC };
                    //mailResult = SendFromOutlook(mailBody, toMails,"More4u Account",ccMails).Result;
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
