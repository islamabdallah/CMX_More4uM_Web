using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
    public interface IBenefitMailService
    {
        Task<List<BenefitMailModel>> GetAllBenefitMails();
        bool CreateBenefitMail(BenefitMailModel model);
        Task<bool> UpdateBenefitMail(BenefitMailModel model);
        bool DeleteBenefitMail(long id);
        BenefitMailModel GetBenefitMail(long id);
        BenefitMailModel GetBenefitMailsByBenefitId(long benefitId);
        bool SendToMailList(BenefitRequestModel benefitRequestModel, List<string> groupMails = null, List<GroupEmployeeModel> groupEmployeeModels = null);
    }
}
