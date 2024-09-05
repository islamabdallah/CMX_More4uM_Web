using MoreForYou.Services.Models.MasterModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MoreForYou.Services.Contracts
{
    public interface IEmployeeRequestService
    {
        List<EmployeeRequestModel> GetAllEmployeeRequests();
        EmployeeRequestModel CreateEmployeeRequest(EmployeeRequestModel model);
        Task<bool> UpdateEmployeeRequest(EmployeeRequestModel model);
        bool DeleteEmployeeRequest(long id);
        List<EmployeeRequestModel> GetEmployeeRequest(long requestId);
        EmployeeRequestModel GetEmployeeRequestByEmployeeNumber(long employeeNumber);

    }
}
