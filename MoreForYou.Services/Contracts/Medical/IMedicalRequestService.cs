using Microsoft.AspNetCore.Http;
using MoreForYou.Models.Models.MedicalModels;
using MoreForYou.Services.Models.API.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts.Medical
{
    public interface IMedicalRequestService
    {
        Task<MedicalRequest> CreateMedicalRequest(MedicalRequest model);

        Task<MedicalRequest> CreateMedicalRequestModelAsync(MedicalRequestApiModel requestAPI);

        Task<MedicalRequestLog> CreateMedicalRequestLog(MedicalRequest requestAPI);

        Task<MedicalAttachment> CreateMedicalRequestAttachment(
          MedicalRequest requestAPI,
          List<IFormFile> files);

        Task<string> CreateMedicalRequestAttachmentt(MedicalRequest requestAPI, List<IFormFile> files);

        bool SendRequestToMedicalAdminRole(MedicalRequest model);

        bool SendRequestToDoctorRoleAsync(MedicalRequest model);

        MedicalRequest addMedicalRequest(MedicalRequest model, List<IFormFile> files);

        PendingRequestApiModel GetAllMedicalRequestsByType(int TypeId, int langCode);

        PendingRequestApiModel GetEmployeeMedicalRequestsBy(long EmployeeNumber, int langCode);

        PendingRequestApiModel GetAllEmployeeMedicalRequests(MedicalRequestSearch searchModel);

        PendingRequestApiModel GetAllMedicalRequestsByAdmin(long EmployeeNumber, int langCode);

        MedicalRequestModel GetMedicalRequestsDetailsById(long MedicalRequestId, int langCode);

        MedicalResponseModel GetMedicalResponseForEmployee(long MedicalRequestId, int langCode);

        MedicalResponseModel GetMedicalResponseForDoctor(long MedicalRequestId, int langCode);

        Task<MedicalRequestDetailsModel> GetMedicalRequestsDetailsAsync(
          long MedicalRequestId,
          long EmployeeNumber,
          int langCode);

        List<string> GetRequestAttachmentsByStatus(long requestId, string status);
    }
}
