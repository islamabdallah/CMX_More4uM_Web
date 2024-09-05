using MoreForYou.Services.Models.API;
using MoreForYou.Services.Models.MasterModels;
using MoreForYou.Services.Models.MaterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
    public interface IBenefitRequestService
    {
        Task<List<BenefitRequestModel>> GetAllBenefitRequestsByCountry(string country);
        Task<List<BenefitRequestModel>> GetAllBenefitRequests();
        Task<BenefitRequestModel> CreateBenefitRequest(BenefitRequestModel model);
        Task<bool> UpdateBenefitRequest(BenefitRequestModel model);
        bool DeleteBenefitRequest(long id);
        BenefitRequestModel GetBenefitRequest(long id);
        public Task<List<BenefitRequestModel>> GetBenefitRequestByEmployeeId(long employeeNumber);
        Task<List<BenefitRequestModel>> GetBenefitRequestByBenefitId(long benefitId);
       
        bool CancelBenefitRequest(
            BenefitRequestModel benefitRequestModel, 
            RequestWokflowModel requestWokflowModel);

        public Task<List<BenefitRequestModel>> GetBenefitRequestByEmployeeId(
            long employeeNumber, 
            long benefitId);

        BenefitRequestModel GetBenefitRequestByGroupId(long groupId);
        int GetTimesEmployeeReceieveThisBenefit(long employeeNumber, long benefitId);
        Request CreateRequestAPIModel(BenefitRequestModel benefitRequestModel);
        //Task<string> SendReuestToWhoIsConcern(long benefitRequetId, int orderNumber);
        Request CreateRequestModel(RequestAPI requestAPI, long benefitTypeId, bool isGift);
        bool ISEmployeeHasHoldingRequestsForthisBenefit(long employeeNumber, long benefitId);

        List<BenefitRequestModel> GetRequestsSendToMe(long employeeNumber);
        bool EmployeeCanRedeemthisbenefit(BenefitModel benefitModel, EmployeeModel employeeModel);
        public Request ConvertFromWebRequestToRequestModel(WebRequest webRequest);


    }
}
