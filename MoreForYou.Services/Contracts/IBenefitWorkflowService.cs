using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
    public interface IBenefitWorkflowService
    {
        Task<List<BenefitWorkflowModel>> GetAllBenefitWorkflows();
        Task<bool> CreateBenefitWorkflow(BenefitWorkflowModel model);
        Task<bool> UpdateBenefitWorkflow(BenefitWorkflowModel model);
        bool DeleteBenefitWorkflow(long id);
        BenefitWorkflowModel GetBenefitWorkflow(long id);
        BenefitWorkflowModel GetBenefitWorkflow(long benefitId, string roleId);

        Task<List<BenefitWorkflowModel>> GetBenefitWorkflowByName(BenefitWorkflowModel model);

        List<BenefitWorkflowModel> GetBenefitWorkflowS(long BenefitId);
        List<string> CreateBenefitWorkFlow(BenefitModel model, int languageId);

    }
}
