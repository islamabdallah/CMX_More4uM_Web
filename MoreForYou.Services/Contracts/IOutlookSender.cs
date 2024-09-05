using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
    public interface IOutlookSenderService
    {
        Task<bool> SendFromOutlook(string body, List<string> toMails, string subject, string[] ccMails = null);

        string PrapareMailBody(BenefitRequestModel model);
        Task<bool> SendMailForApproval(string To, BenefitModel benefitModel, EmployeeModel requester, long benefitType, string CC = null);
        string PrapareGroupMailBody(string BenefitImage, string benefitName, DateTime from, DateTime to, List<GroupEmployeeModel> groupEmployeeModels);

    }
}
