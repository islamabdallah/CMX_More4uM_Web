using Microsoft.AspNetCore.Http;
using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
    public interface IBenefitService
    {
        Task<List<BenefitModel>> GetAllBenefits();
        BenefitModel CreateBenefit(BenefitModel model);
        Task<bool> UpdateBenefit(BenefitModel model);
        bool DeleteBenefit(long id);
        Task<BenefitModel> GetBenefit(long id);
        Task<List<BenefitModel>> GetBenefitByName(BenefitModel model);
        List<BenefitModel> BenefitsUserCanRedeem(List<BenefitModel> benefitModels, EmployeeModel employeeModel);
        BenefitConditionsAndAvailable CreateBenefitConditions(BenefitModel benefitModel, EmployeeModel employeeModel);
        BenefitAPIModel CreateBenefitAPIModel(BenefitModel model, int languageId);
        Task<List<BenefitAPIModel>> GetMyBenefits(long employeeNumber, int languageId);

        Task<List<Participant>> GetEmployeesCanRedeemThisGroupBenefit(long employeeNumber, long benefitId);

        Task<HomeModel> ShowAllBenefits(EmployeeModel employeeModel, int languageId);
        Task<BenefitAPIModel> GetBenefitDetails(long benefitId, EmployeeModel employeeModel, int languageId);
        Task<List<Participant>> GetEmployeesWhoCanIGiveThisBenefit(long employeeNumber, long benefitId);
        Task<WebRequest> BenefitRedeem(long benefitId, string userId);
        string CalculateWorkDuration(DateTime joinDate);
        Task<List<BenefitModel>> GetBenefitsByAddress(string country);

        public BenefitConditionsAndAvailable CreateArabicBenefitConditions(BenefitModel benefitModel, EmployeeModel employeeModel);
    }
}
