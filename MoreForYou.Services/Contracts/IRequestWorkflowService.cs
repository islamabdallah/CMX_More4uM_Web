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
    public interface IRequestWorkflowService
    {
        Task<List<RequestWokflowModel>> GetAllRequestWorkflows();
        Task<RequestWokflowModel> CreateRequestWorkflow(RequestWokflowModel model);
        Task<bool> UpdateRequestWorkflow(RequestWokflowModel model);
        bool DeleteRequestWorkflow(long id);
        Task<List<RequestWokflowModel>> GetRequestWorkflowByEmployeeNumber(long employeeNumber);
        Task<List<RequestWokflowModel>> GetRequestWorkflow(long requestId);
        Task<bool> CancelRequestWorkflow(RequestWokflowModel requestWokflowModel);
        RequestWokflowModel GetRequestWorkflowByEmployeeNumber(long employeeNumber, long requestId);

        List<RequestWokflowModel> GetGroupRequestWorkflowByEmployeeNumber(long employeeNumber);
        Task<ManageRequest> CreateManageRequestFilter(string userId, ManageRequest manageRequest);
        List<Request> CreateRequestToApprove(List<RequestWokflowModel> requestWokflowModels, int languageId);
        List<RequestWokflowModel> CreateWarningMessage(List<RequestWokflowModel> requestWokflowModels);
        List<RequestWokflowModel> EmployeeCanResponse(List<RequestWokflowModel> requestWokflowModels);
        Task<string> CancelMyRequest(long id);
        Task<List<Request>> GetMyBenefitRequests(long employeeNumber, long BenefitId, long benefitTypeId, int languageId);
        List<Request> CreateRequestToApprove(List<BenefitRequestModel> benefitRequestModels, long employeeNumber, int languageId);
        Task<string> AddIndividualRequest(Request request, string userId, BenefitModel benefitModel);
        Task<string> SendReuestToWhoIsConcern(long benefitRequetId, int orderNumber);
        Task<string> UploadedImageAsync(IFormFile ImageName, string path);
        bool SendRequestToHRRole(BenefitRequestModel benefitRequestModel);
        bool SendRequestToTimingRole(BenefitRequestModel benefitRequestModel);
        Task<string> ConfirmGroupRequest(Request request, string userId, BenefitModel benefitModel);

        Task<bool> SendNotification(BenefitRequestModel benefitRequestModel, RequestWokflowModel DBRequestWorkflowModel, string type, long responserId);

        Task<bool> SendNotificationToGroupMembers(List<GroupEmployeeModel> groupEmployeeModels, RequestWokflowModel DBRequestWorkflowModel, BenefitRequestModel benefitRequestModel, string notificationMessage, string arabicNotificationMessage, string type);

        NotificationModel CreateNotification(string Type, long employeeNumber, long BenefitRequestId, string message, string arabicMessage, long responsedBy, long requestWorkflowId);

        Task<bool> SendToSpecificUser(string message, RequestWokflowModel model, string requestType, string employeeName, string userId);

        Task<string> AddDocumentsToRequest(long requestNumber, List<IFormFile> files);
        ManageRequest CreateManageRequestModel(RequestSearch requestSearch);
        Task<List<Gift>> GetMyGifts(long employeeNumber, int languageId);
        string CalculateWorkDuration(DateTime joinDate);
        Task<bool> GroupRequestResponse(long groupId, int status, string message, long employeeNumber);

        int CheckSameWorkflow(int Currentorder, long benefitId, RequestWokflowModel requestWokflowModel, long employeeNumber);
        bool CreateAutoRequestWorkflowForRole(string nextRoleId, RequestWokflowModel requestWokflowModel, List<EmployeeModel> employeeModels);
        RequestWokflowModel GetRequestWorkflowById(long requestWorflowId);

        Task<bool> AddSameResponseForAllHRRole(long requestNumber, int status, string message, long responsedBy);
        Task<bool> AddSameResponseForAllTimingRole(long requestNumber, int status, string message, long responsedBy);

        Task<string> SaveImage(string strm, string uploadFolder);

        RequestDates CalculateRequestExactDates(BenefitModel benefitModel, EmployeeModel CurrentEmployee);


        Task<bool> SendMailToWhoIsConcern(BenefitRequestModel benefitRequestModel, RequestWokflowModel requestWokflowModel);


    }
}
