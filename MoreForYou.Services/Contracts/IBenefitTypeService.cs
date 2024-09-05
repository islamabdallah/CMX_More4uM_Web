using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
    public interface IBenefitTypeService
    {
        List<BenefitTypeModel> GetAllBenefitTypes();
        BenefitTypeModel CreateBenefitType(BenefitTypeModel model);
        Task<bool> UpdateBenefitType(BenefitTypeModel model);
        bool DeleteBenefitType(long id);
        BenefitTypeModel GetBenefitType(long id);
        Task<List<BenefitTypeModel>> GetBenefitTypeByName(BenefitTypeModel model);
    }
}
